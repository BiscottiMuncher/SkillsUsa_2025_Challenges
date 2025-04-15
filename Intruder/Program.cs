using System.Text;
using System.Net;
using System.Net.Sockets;

class Program{
    public static void Main(string[] args){
        TcpClientHandler();    
    }

    //Stolen Try catch for the write
    static void postNetMessage(NetworkStream networkStream, string message){
        byte[] srvResponse = Encoding.Default.GetBytes(message);
        try
        {
            networkStream.Write(srvResponse, 0, srvResponse.Length);

        }
        // Was running into issues where disconnect through netcat would break server
        catch (IOException ex)
        {
            Console.WriteLine("IOException: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
    }


    static void createLogger(string lpath){
        // Start Logger
        string loggerFilePath = lpath;
        if (File.Exists(loggerFilePath)){
            Console.WriteLine("File exists at: " + loggerFilePath);
        }else{
            // If the file doesnt exist create it
            using (FileStream fs = File.Create(loggerFilePath)) {
                byte[] preamble = new UTF8Encoding(true).GetBytes("TCP LISTEN SERVER LOGS\n");
                fs.Write(preamble, 0, preamble.Length) ;
            }
        }
    }


   static void writeToLogger(string loggerPath, string message){
        if (File.Exists(loggerPath)){
            using (StreamWriter writer = new StreamWriter(loggerPath, true)){            
                writer.WriteLine(DateTime.Now +": "+ message);
            }
        }else{
            Console.WriteLine("File Does not exist");
            createLogger(loggerPath);
        }
   } 


    static void TcpClientHandler(){
        // Start Server Block 
        TcpListener srv = new TcpListener(IPAddress.Any, 259);
        srv.Start();
        string loggerActualPath = @"C:\Skills\ServerListenLog.log";
        createLogger(loggerActualPath);
        Console.WriteLine("Starting Listen Server at: " + srv.LocalEndpoint);


        // Create Client and Start listen block
            while (true){
                TcpClient client = srv.AcceptTcpClient();
                NetworkStream networkStream = client.GetStream();

        // TCP listen block
            if(client.Connected){

                Console.WriteLine(client.Client.RemoteEndPoint + " Connected to server");
                writeToLogger(loggerActualPath, client.Client.RemoteEndPoint + " | Connected");

                 while(client.Connected){
                    byte[] message = new byte[1024];
                    int bytesRead = networkStream.Read(message, 0, message.Length);
                    string byteString = Encoding.UTF8.GetString(message, 0, bytesRead).Trim(); 
                    writeToLogger(loggerActualPath, byteString);
                    // With non graceful disconnect two blank messages are posted, this can be sen as disconect
                        // Maybe add a zero check here to make sure it doesnt spit our random shit
                        if(byteString.Equals("bye")){
                            //Graceful Disconnect
                            postNetMessage(networkStream, "Goodbye\n");
                            client.Close();
                        }
                        else if(byteString.Equals("help")){
                            postNetMessage(networkStream, "Commands:\n ls \n get \n bye \n \n");
                        }
                        else if(byteString.Equals("ls")){
                            postNetMessage(networkStream, "Files:\n welcome.txt \n ssl.crt \n passwd.enc \n notes.txt \n \n");
                        }
                        /// Super cheesy method inbound for this
                       else if(byteString.Equals("get")){
                            postNetMessage(networkStream, " > Error; No File Selected..\n");
                            postNetMessage(networkStream, "Usage: get example.txt\n \n");
                        }
                        else if(byteString.Equals("get welcome.txt")){
                            postNetMessage(networkStream, " > Downloading File...\n");
                            postNetMessage(networkStream, "Welcome to SUSA NET07 SAN.\n \n");
                        }
                        else if(byteString.Equals("get ssl.crt")){
                            postNetMessage(networkStream, " > Downloading File...\n");
                            postNetMessage(networkStream, "-----BEGIN SUSA PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAA\nSCBKcwggSjAgEAAoIBAQCwECB5/Xz/\nKAa8vv5bK2AJuXAQ1ZypUvDKcE9MCg\ngcwTej1F9NK8050sTp9stlInT5PIKT\nU5oSUtLGkBH98fed0YnYn6cdmNSUIg\nSj8ypUvDK13/WW3kLhGf1Bqsae6f9=\n-----END SUSA PRIVATE KEY-----\n \n");
                            // Red herring
                        }
                        else if(byteString.Equals("get passwd.enc")){
                            postNetMessage(networkStream, " > Downloading File...\n");
                            postNetMessage(networkStream, "FA9BEB99E4029AD5A6615399E7BBAE21356086B3\n \n");
                            //Actual file to be decrypted
                        }
                        else if(byteString.Equals("get notes.txt")){
                            postNetMessage(networkStream, " > Downloading File...\n");
                            postNetMessage(networkStream, "Admin Notes \nMoving all the files off this network to the new one. \nCleanup will continue tomorrow. \n  - IT \n \n");
                            // Hints
                        }
                        else if (bytesRead > 50){
                            postNetMessage(networkStream, "BUFFER OVERFLOW: 0x135017C\n");
                            client.Close();
                        }else{
                            postNetMessage(networkStream, " > Please enter a valid command\n Example: help \n \n");
                        }
                    }

                }
                    client.Close();
            }
        }
    }