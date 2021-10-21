import socket
import sys
import time 
HOST = "localhost"

while True:
    try:
        PORT = int(input("Enter the port you wish to connect to: "))
        break;
    except:
        print("Port must be an integer.")
# Create a socket (SOCK_STREAM means a TCP socket)
try:
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as sock:
        # Connect to server and send data
        sock.connect((HOST, PORT))
        print("Connection established.")
        # Receive data from the server and shut down
        while True: 
            data = input("Send command (type q or Q to Quit): ")
            
            if (len(data) == 0) or (data[len(data)-1] != "$"):
                data += "$"

            sock.send(data.encode())
            if data.lower() == "q$":
                print("Closing connection.")
                break;
            else:
                received = str(sock.recv(65536), "utf-8")
                print("Received: {}".format(received))

except (ConnectionResetError, ConnectionAbortedError) as e:
    print("The connection was closed.")
    
