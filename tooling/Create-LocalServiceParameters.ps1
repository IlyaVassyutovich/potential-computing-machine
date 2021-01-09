$private:Password = ConvertTo-SecureString -String "Irrelevant" -AsPlainText -Force
$private:ServiceAccount = New-Object -TypeName System.Management.Automation.PSCredential `
    -ArgumentList "NT AUTHORITY\SYSTEM", $Password

$private:PublishDestination = "c:\Program Files\potential-computing-machine\"

$global:PublishParameters = @{
    ServiceAccount = $ServiceAccount;
    PublishDestination = $PublishDestination
}