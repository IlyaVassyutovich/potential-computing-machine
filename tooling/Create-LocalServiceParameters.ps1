$private:Password = ConvertTo-SecureString -String "qwerty" -AsPlainText -Force
$private:ServiceAccount = New-Object -TypeName System.Management.Automation.PSCredential `
    -ArgumentList "HYPERION\PCM-Service", $Password

$private:PublishDestination = "c:\ProgramData\potential-computing-machine\"

$global:PublishParameters = @{
    ServiceAccount = $ServiceAccount;
    PublishDestination = $PublishDestination
}