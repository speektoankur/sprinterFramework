# Sprinter Framework (BE + FE Functional Tests)

## High level Test Run Flow 
<img width="1221" alt="Screenshot 2024-10-21 at 12 33 14 AM" src="https://github.com/user-attachments/assets/eb3c4d98-9ea5-432a-bb91-bef931320051">

## Test Run Execution Recording 


https://github.com/user-attachments/assets/d60ba626-a114-4655-a726-b29cc829f53d

### Prerequisites 

1. Install Visual Studio Code
2. Install the .NET SDK (https://dotnet.microsoft.com/en-us/download)
3. Check Dotnet version -> running command in Terminal "dotnet --version"

### Test Run Executions

1. Front End Executions ->
                          dotnet test --filter "TestCategory=UI"
2. Back End Executions ->
                          dotnet test --filter "TestCategory=API"
