#include <iostream>
#include <cstring>
#include <cstdio>
#include <stdarg.h>
#include <stdio.h>
#include <ctype.h>
#include <unistd.h>
#include <string.h>
#include <stdlib.h>

using namespace std;


void shell(){
	system("/bin/bash");
}

void do_get(FILE* request, FILE* response) {
  char page[1024];
  fgets(page, 1024, request);

  char buffer[1024];
  strcat(buffer, "The page \"");
  strcat(buffer, page);
  strcat(buffer, "\" was not found.");

  fputs(buffer, response);
}
void sqlInject(char *userName){
  char query1[1000] = {0};
  sprintf(query1, "SELECT UID FROM USERS where name = \"%s\"", userName);
 
}



int main(int argc, char *argv[])
{
    char name[30];
    strcpy(name, argv[1]);
    cout << "Welcome Mr." << name << endl;
   
    return 0;
}
