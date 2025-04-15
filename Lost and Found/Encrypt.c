#include <stdio.h>
#include <stdlib.h>

void encrypt(char *in_fp){
    printf("~0x7E9 ENCRYPTION ENGINE~\n");
    FILE *fp, *out_fp;
    fp = fopen(in_fp, "r");
    int ch;

    if (fp != NULL){
        out_fp = fopen("out.txt", "w");
        printf(" >Starting encryption");
        while ((ch = fgetc(fp)) != EOF)
        {            
            fputc(ch+25, out_fp);
        }
        printf("\n >Finished encryption");
        fclose(fp);
        fclose(out_fp);
        
    }else {
        printf("!!File is NULL: Quitting!!");
    } 

}

int main(int argc, char* argv[]) {
    if(argc < 2){
        printf("No File Passed!\n");
        printf(" Example: ./Encrypt example.txt");
        return 1;
    }

    encrypt(argv[1]);
    return 0;
}