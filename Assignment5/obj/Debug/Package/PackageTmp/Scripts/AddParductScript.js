function Validate()
{
    
    var name = document.getElementById("name").value;
    var price = document.getElementById("price").value;
    var description = document.getElementById("description").value;
    if(name==""||price==""||description=="")
    {
        alert("Must Fill Mandaory fields Operation Failed");
        window.location.reload();
    }
}
function ValidateUpdatio()
{
    var name = document.getElementById("name").value;
    var pass = document.getElementById("password").value;
    var login = document.getElementById("login").value;
    var picture = document.getElementById("picture").value;
    if (name == "" || pass == "" || login == ""||picture=="") {
        alert("Must Fill Mandaory fields Operation Failed");
        if (name == "" || price == "" || description == "") {
            alert("Must Fill Mandaory fields Operation Failed");
            window.location.reload();
        }
    }
}