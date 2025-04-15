#include <stdio.h>
#include <stdlib.h>

void decrypt(char *in_fp){
    printf("~0x7E9 DECRYPTION ENGINE~\n");
    FILE *fp, *out_fp;
    fp = fopen(in_fp, "r");
    int ch;

    if (fp != NULL){
        char outPutName[200]; 
        snprintf(outPutName, sizeof(outPutName), "%s_decrypted.txt", in_fp);
        out_fp = fopen(outPutName, "w");
        printf(" >Starting decryption");
        while ((ch = fgetc(fp)) != EOF)
        {            
            fputc(ch-25, out_fp);
        }
        printf("\n >Finished decryption");
        fclose(fp);
        fclose(out_fp);
        
    }else {
        printf("!!File is NULL: Quitting!!");
    } 

}

int main(int argc, char* argv[]) {
    if(argc < 2){
        printf("No File Passed!\n");
        printf(" Example: ./Decrypt example.txt");
        return 1;
    }

    decrypt(argv[1]);
    return 0;
}