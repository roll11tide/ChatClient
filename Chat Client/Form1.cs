using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Chat_Client
{
    public partial class Form1 : Form
    {
        #region Global Variables
        // All these variables are static to avoid cross-thread issues

        // Main client socket
        private static System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        // Main server stream used to, well, connect to the server
        private static NetworkStream serverStream = default(NetworkStream);

        // readStreamData is the variable that is used to transfer the stream data across threads
        string readStreamData = "";

        // If the stream dies, then unbroken is set to false and the loop that reads input breaks
        public static bool unbroken = true;

        // This is uesd to measure null characters later in order to avoid freezing from an empty socket
        int nullResponseCounter = 0;

        // This is the variables that stores the server password so it can encrypt messages later
        public static string serverPassword = "";
        #endregion

        public Form1()
        {
            InitializeComponent();

            // Fills username field with default random username (see generateUsername() for more details)
            usernameField.Text = generateUsername();

            // Sets the server password field to be censored with an asterisk
            passwordField.PasswordChar = '*';

            // This counts the revision number
            string revisionNumber = "8";

            // Initial information, proprietary stuff, and a no warranty warning (see message() for more details)
            readStreamData = "Chat client developed by Jesse Wells. Revision " + revisionNumber + " Edition 1.0";
            message();
            readStreamData = "This is a closed source project in alpha. Please report any and all bugs to the author, Jesse Wells.";
            message();
            readStreamData = "---This program is provided AS IS with NO warranty. Use at your own risk.---";
            message();
        }

        #region Methods
        private void getMessage()
        {
            while (unbroken)
            {
                try {
                    // Gets server's stream
                    serverStream = clientSocket.GetStream();

                    // Get's the stream's buffer size
                    byte[] inStream = new byte[clientSocket.ReceiveBufferSize];

                    // Reads from the stream when data is coming down the pipe
                    serverStream.Read(inStream, 0, inStream.Length);

                    // Converts from bytes to string
                    readStreamData = System.Text.Encoding.ASCII.GetString(inStream);

                    // Remove null characters
                    readStreamData = readStreamData.Replace("\0", String.Empty);

                    // This variable is used to check whether or not the decryption was successful later
                    bool goodDecrypt = true;

                    // Make sure we're not just reading from the void
                    if (!checkForEmptyResponse(readStreamData))
                    {
                        // Good response - not blank
                        if (!String.IsNullOrWhiteSpace(serverPassword))
                        {
                            // Message is almost certainly encrypted
                            try
                            {
                                // I'm trying to make this as grief-proof as possible
                                if (!String.IsNullOrWhiteSpace(serverPassword))
                                {
                                    // Decrypt with stored server password if the stored server password is not blank
                                    // If the password is blank, then the server does not have a password and therefore will not send encrypted data
                                    readStreamData = AES.Decrypt(readStreamData, serverPassword);
                                }
                            }
                            catch (Exception e)
                            {
                                // This is a very generic error - it can occur whenever the client fails to decrypt an incoming message
                                // This can be due to a variety of problems including a somehow mismatched server password, an incorrect password, or simply a serverside problem
                                // There aren't any known serverside problems that can cause this error as of 4/22/2016 at 6:24 AM (the final debug test)

                                // Print error to chat box (see message() for details)
                                readStreamData = "Invalid encryption key/decrkyption/server password.";
                                message();

                                // Flushes the connction to avoid more problems and a potential security issue (see flushConnection() for more details)
                                flushConnection();

                                // Decryption failed, so this variable is set to false
                                goodDecrypt = false;
                            }
                        }

                        // Prevents duplicate error messages - the user doesn't need to see it twice
                        if (goodDecrypt)
                        {
                            // Make sure the server didn't send the client the que to close its socket
                            if (checkForDisconnectionSequence(readStreamData))
                            {
                                // It did
                                readStreamData = readStreamData.Replace("$$$", "");
                                message();
                            }
                            else
                            {
                                // Loop through all the (DOLLARSIGN) markers and replace them with actual dollar signs
                                // The logic behind this is explained in the server under Broadcasting.cs
                                // Essentially, it is a way of sending the delimiter (a dollar sign) back and forth between the server and client without actually using the delimiter character
                                // What used to happen was when the client typed an actual dollar sign, it would cut the message off at the dollar sign because it thought that was the delimiter
                                // By converting a $ to (DOLLARSIGN), the client and server avoid this problem
                                while (readStreamData.Contains("(DOLLARSIGN)"))
                                {
                                    readStreamData = readStreamData.Replace("(DOLLARSIGN)", "$");
                                }

                                // Send message to chat box
                                message();
                            }
                        }

                        // If the window is minimized of not active, it will blink orange
                        if (this.WindowState == FormWindowState.Minimized | Form.ActiveForm != this)
                        {
                            // If the debugger is attached, this will cause an error for some reason
                            if (!Debugger.IsAttached)
                            {
                                // See FlashWindow for more information - this just makes the icon blink orange
                                FlashWindow.Start(this);
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    // For whatever reason, the socket was either lost or closed
                    // Let the user know
                    readStreamData = "Connection lost.";
                    message();

                    // Break the loop
                    unbroken = false;

                    // Properly close the socket
                    flushConnection();
                }
            }

            if (!unbroken)
            {
                // If the loop is broken, close the socket
                flushConnection();
            }
        }

        private void message()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new MethodInvoker(message));
                } catch
                {
                    // Disposed object error - doesn't matter
                }
            } else
            {
                try
                {
                    chatHistory.Text = chatHistory.Text + "\r\n >> " + readStreamData;
                    chatHistory.SelectionStart = chatHistory.Text.Length;
                    chatHistory.ScrollToCaret();
                } catch
                {
                    // Cannot access disposed object on exit error
                    // It doesn't matter since the program is exiting anyway
                }
            }
        }

        private bool checkForDisconnectionSequence(string input)
        {
            int counter = 0;

            foreach (char character in input)
            {
                if (character == '$')
                {
                    counter++;
                }

                if (counter > 2)
                {
                    // Disconnects client and breaks all methods calling this
                    flushConnection();
                    return true;
                }
            }

            // Returns true if no disconnection sequence was found
            return false;
        }

        private bool checkForEmptyResponse(string data)
        {
            bool returnbool = false;
            if (data == "")
            {
                nullResponseCounter++;
                returnbool = true;
            }

            if (nullResponseCounter >= 3)
            {
                unbroken = false;
                nullResponseCounter = 0;
                readStreamData = "The connection has been timed out after three empty server responses.";
                message();
                flushConnection();
            }

            return returnbool;
        }

        // This cleanly closes the client's socket
        private static void flushConnection()
        {
            // Completely reinitializing the socket and network stream after closing it seems to fix the reconnection problem
            clientSocket.Close();
            clientSocket = new System.Net.Sockets.TcpClient();
            serverStream = default(NetworkStream);
        }

        // This is used to send a message to the server
        private void sendMessage()
        {
            try
            {
                // Make a string to edit the text so we don't have to actual edit the value of the textbox
                string editedText = typedText.Text;

                // Clean up the data
                editedText = editedText.Replace("\r\n", "");
                editedText = editedText.Replace("$", "(DOLLARSIGN)");
                
                // If there is a server password stored in memory
                if (!String.IsNullOrWhiteSpace(serverPassword))
                {
                    // Encrypt message before sending
                    editedText = AES.Encrypt(editedText, serverPassword);
                }

                // Turn the message with the delimiter at the end to bytes
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(editedText + "$");

                // Write bytes to server stream
                serverStream.Write(outStream, 0, outStream.Length);

                // Flush server stream so that it can be used without a problem later
                serverStream.Flush();

                // Clear out the textbox so another message can be typed easily
                typedText.Text = null;
            }
            catch
            {
                // This catch is almost exclusively triggered by not being connected to a server
                // Let the user know they are not connected to a server
                readStreamData = "You are not connected to a server.";
                message();

                // Clear out the textbox so the user can type another message
                typedText.Text = null;
            }
        }

        // This is used to generate the default username that the program autofills when it runs
        private string generateUsername()
        {
            // String used to store the username later
            string returnUsername = "";

            // A variable of type random used to generate numbers that correlate to letters later
            Random random = new Random();

            // Character array of all the letters in the english alphabet
            char[] letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            // Generates a username that is 8 characters long and will keep adding characters until it meets that length
            while (returnUsername.Length <= 8)
            {
                // Generate a 0 or 1
                if (random.Next(0, 2) == 0)
                {
                    // 50% chance to add letter
                    returnUsername += letters[random.Next(0, 26)];
                } else
                {
                    // 50% chance to add number
                    returnUsername += random.Next(0, 10);
                }
            }

            // Return the generated username
            return returnUsername;
        }
        #endregion

        #region Event Handlers
        private void connectButton_Click(object sender, EventArgs e)
        {
            // Why would a user need to connect to a server when it is already connected to one?
            if (clientSocket.Connected)
            {
                readStreamData = "You are already connected to a server.";
            } else
            {
                // This is used to store the port later
                int parsedPort;

                // Make sure all the forms are filled with actual data
                if (!String.IsNullOrWhiteSpace(ipField.Text) & !String.IsNullOrWhiteSpace(portField.Text) & !String.IsNullOrWhiteSpace(usernameField.Text) & (int.TryParse(portField.Text, out parsedPort)))
                {
                    try
                    {
                        // These are used to check whether the server is responding
                        var result = clientSocket.BeginConnect(ipField.Text, parsedPort, null, null);
                        var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500));

                        if (success)
                        {
                            // Stop trying to connect if it's already connected
                            clientSocket.EndConnect(result);

                            // The unbroken variable keeps the connection socket open
                            unbroken = true;

                            // Get the server's stream
                            serverStream = clientSocket.GetStream();

                            // Get the byte size of an empty string
                            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("");

                            // The client will be disconnected by the server if the password is incorrect, so it is safe to allow the client to assume whatever it passes is correct, because if it is correct the socket will stay open and the connection will not be reset
                            // If the password field is blank, then it is assumed the server has no password
                            if (String.IsNullOrWhiteSpace(passwordField.Text))
                            {
                                // No password
                                // Broadcast unencrypted with delimiter
                                outStream = System.Text.Encoding.ASCII.GetBytes(usernameField.Text + "$");
                                serverPassword = "";
                            } else
                            {
                                // There is a password
                                // Purify data
                                passwordField.Text = passwordField.Text.Replace("\r\n", "");
                                passwordField.Text = passwordField.Text.Replace(" ", "");

                                // The dataPackage variable makes it easier to encrypt the message as a whole
                                string dataPackage = usernameField.Text + " " + passwordField.Text;

                                // Encrypt the whole thing
                                dataPackage = AES.Encrypt(dataPackage, passwordField.Text);

                                // Add the delimiter and convert the whole thing to bytes
                                outStream = System.Text.Encoding.ASCII.GetBytes(dataPackage + "$");

                                // Set the serverPassword to what is passed by the user
                                serverPassword = passwordField.Text;
                            }
                            
                            // Write bytes to server stream
                            serverStream.Write(outStream, 0, outStream.Length);

                            // Flush server stream for later use
                            serverStream.Flush();

                            // Create and start the thread that listens for incoming messages
                            Thread ctThread = new Thread(getMessage);
                            ctThread.Start();

                            // Make the active control the typedText field so they can start typing their first message more easily
                            this.ActiveControl = typedText;
                        } else
                        {
                            // Failed to connect - flush connection and tell the user the server is not responding
                            flushConnection();
                            readStreamData = "The server is not responding.";
                            message();
                        }
                    }
                    catch (Exception f)
                    {
                        // NOTICE - This can fire if the server takes too long to respond, not necessarily not responding at all
                        // This almost never fires anymore, but it can still happen so this catch is necessary
                        readStreamData = "Houston, we have a problem.\r\n" + f.ToString();
                        message();
                    }
                }
                else
                {
                    // A control was empty - this finds out which one so the user can get specific input about which field is blank
                    foreach (Control control in Controls)
                    {
                        // Finds which TextBox is blank
                        if ((control is TextBox) & (control.Text == "") & (control.Name == "usernameField" | control.Name == "ipField" | control.Name == "portField"))
                        {
                            // The control that was blank was found
                            MessageBox.Show(control.Name + " is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }

                        // This checks to see if the port field is not an integer
                        if (!int.TryParse(portField.Text, out parsedPort))
                        {
                            // The port field is not an integer
                            MessageBox.Show("The port field is not a number.", "Port Is Not a Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            // This button simply calls the sendMessage() method
            // See sendMessage() for more details
            sendMessage();
        }

        private void typedText_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Checks to see if the user pressed enter, and if so, will send the message using sendMessage()
            // See sendMessage() for more details
            if (e.KeyChar == (char)Keys.Enter)
            {
                sendMessage();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Before termination of program, a disconnection sequence is released
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("!RELEASECLIENT" + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            } catch
            {
                // This runs if the server is disconnected previous to exit
            }

            // This closes all threads
            //Environment.Exit(Environment.ExitCode);
            Application.Exit();
        }

        private void clearChatHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clears the chat history
            chatHistory.Text = null;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Sends the command that asks the server to disconnect the client's socket
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("!RELEASECLIENT" + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                // Flush the socket on the client's end
                flushConnection();

                // Let the user known the sockets have been closed
                readStreamData = "Connection severed and sockets flushed.";
                message();
            } catch
            {
                // Cannot disconnect from a server when the socket is not connected to begin with
                readStreamData = "You are not connected to a server.";
                message();
            }
        }

        private void showPasswordButton_MouseDown(object sender, MouseEventArgs e)
        {
            // Show the server password field while the user is holding the button down
            passwordField.PasswordChar = (char)0;
        }

        private void showPasswordButton_MouseUp(object sender, MouseEventArgs e)
        {
            // Re-censor the server password field when the user stops holding the button
            passwordField.PasswordChar = '*';
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            // When the window is activated, the icon stops flashing if the debugger is not attached
            // When the debugger is attached, this will cause an error, so this only happens when the debugger is not attached
            if (!Debugger.IsAttached)
            {
                FlashWindow.Stop(this);
            }
        }

        private void censorTextButton_Click(object sender, EventArgs e)
        {
            // This button is used to toggle whether the typed text is censored
            // It can be used to censor the user's password in the server, or any other sensitive data
            if (censorTextButton.Text == "-")
            {
                // Uncensored text to make censored
                censorTextButton.Text = "*";
                typedText.PasswordChar = '*';
            }
            else
            {
                // Censored text to make uncensored
                censorTextButton.Text = "-";
                typedText.PasswordChar = (char)0;
            }
        }

        private void chatHistory_Enter(object sender, EventArgs e)
        {
            // This is fired when the user tries to click inside the chat history box
            // Clicking inside the chat history box is pointless, so it just makes the typed text box so that the user can type a message
            // It is also makes the selection length zero so the user can't accidentally select anything (it looks awful when the user selects something)
            chatHistory.SelectionLength = 0;
            ActiveControl = typedText;
        }
        #endregion

        #region Flashing Icon
        public static class FlashWindow
        {
            // Import the DLL
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]

            // Global class bool
            private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

            [StructLayout(LayoutKind.Sequential)]
            private struct FLASHWINFO
            {
                // The size of the structure in bytes.
                public uint cbSize;
                // A Handle to the Window to be Flashed. The window can be either opened or minimized.
                public IntPtr hwnd;
                // The Flash Status.
                public uint dwFlags;
                // The number of times to Flash the window.
                public uint uCount;
                // The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
                public uint dwTimeout;
            }

            // Stop flashing. The system restores the window to its original stae.
            public const uint FLASHW_STOP = 0;

            // Flash the window caption.
            public const uint FLASHW_CAPTION = 1;

            // Flash the taskbar button.
            public const uint FLASHW_TRAY = 2;

            // Flash both the window caption and taskbar button.
            // This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
            public const uint FLASHW_ALL = 3;

            // Flash continuously, until the FLASHW_STOP flag is set.
            public const uint FLASHW_TIMER = 4;

            // Flash continuously until the window comes to the foreground.
            public const uint FLASHW_TIMERNOFG = 12;


            // Flash the spacified Window (Form) until it recieves focus.
            public static bool Flash(System.Windows.Forms.Form form)
            {
                // Make sure we're running under Windows 2000 or later
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
            {
                FLASHWINFO fi = new FLASHWINFO();
                fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
                fi.hwnd = handle;
                fi.dwFlags = flags;
                fi.uCount = count;
                fi.dwTimeout = timeout;
                return fi;
            }

            // Flash the specified Window (form) for the specified number of times
            public static bool Flash(System.Windows.Forms.Form form, uint count)
            {
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, count, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            // Start Flashing the specified Window (form)
            public static bool Start(System.Windows.Forms.Form form)
            {
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, uint.MaxValue, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            // Stop Flashing the specified Window (form)
            public static bool Stop(System.Windows.Forms.Form form)
            {
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_STOP, uint.MaxValue, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            // A boolean value indicating whether the application is running on Windows 2000 or later.
            private static bool Win2000OrLater
            {
                get { return System.Environment.OSVersion.Version.Major >= 5; }
            }
        }
        #endregion
    }
}