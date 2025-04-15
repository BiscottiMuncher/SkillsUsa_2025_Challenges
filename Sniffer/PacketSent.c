#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <arpa/inet.h>


    void packet(char *ARRT){

        int sockfd;
        struct sockaddr_in dest_addr;
        const char *message = "0x7E9 Was Here...";
        const char *ip = ARRT; 
        int port = 2025;                 

        if ((sockfd = socket(AF_INET, SOCK_DGRAM, 0)) < 0) {
            perror("socket creation failed");
            exit(EXIT_FAILURE);
        }

        memset(&dest_addr, 0, sizeof(dest_addr));
        dest_addr.sin_family = AF_INET;
        dest_addr.sin_port = htons(port);
        if (inet_pton(AF_INET, ip, &dest_addr.sin_addr) <= 0) {
            perror("Invalid address or address not supported");
            close(sockfd);
            exit(EXIT_FAILURE);
        }

        if (sendto(sockfd, message, strlen(message), 0, (struct sockaddr *)&dest_addr, sizeof(dest_addr)) < 0) 
        {
            perror("sendto failed");
            close(sockfd);
            exit(EXIT_FAILURE);
        }

        printf("Packet sent to %s\n", ip );

        close(sockfd);
    }

int main(int argc, char* argv[]) {


    for(;;) {
        packet(argv[1]);
        sleep(1);
        printf("Another ");
    }


    return 0;
}
