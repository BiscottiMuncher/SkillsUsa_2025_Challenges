#include<stdio.h>
#include<string.h>

int main(int argc, char *argv[]){
    int pwBool;
    char userInput[12] = "";
    char password[12] = "w0ntGuessM3";

    printf("- SUSA Admin Login -\n");
    printf("Enter a password: ");
    scanf("%s",userInput);

    if(strcmp(userInput, password) == 0){
        printf(":> Welcome Administrator\n");
    }else{
        printf("Password Incorrect!");
    }
    return 0;
}
