function Validate()
{
    
    var name = document.getElementById("name").value;
    var price = document.getElementById("price").value;
    var description = document.getElementById("description").value;
    if(name==""||price==""||description=="")
    {
        alert("Some feild is missing");
       
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
           
        }
    }
}
function comment(obj)
{
    var commentmessage = $(obj).closest(".box").find("#txtcomment").val();
    var pid = $(obj).closest(".box").find("#ppid").text();
    var uid = $(obj).closest(".box").find("#userid").val();
    var data = { "Productid": pid, "Userid": uid, "comment": commentmessage };
   
        var settings = {
            type: "POST",
            dataType: "json",
            url: "/Comment/Addcomment",
            data: data,
            success:function(res)
            {
                if(res.flag==true)
                {
                    alert("comment has been added successfully");
                    location.href = res.urli;
                }
                else
                {
                    alert("Some Error occure comment not been added sucessfully");
                }
            },
            error: function () {
                console.log(res);
                alert("\Failed Operation");
            }
        }
        $.ajax(settings);
       
}
function loadmore(obj1) {
    var obj = null;
    var pid = $(obj1).closest(".box").find("#ppid").text();
    var data = { "productid": pid };
    var settings = {
        type: "POST",
        dataType: "json",
        url: "/Comment/Getcomments",
        data: data,
        success: function (res) {
            if (res.cmt) {
                for (var i = 0; i < res.cmt.length; i++) {
                    var obj = res.cmt[i];
                    var image = $("<img wigth=100 height=80 src='/Content/Images/" + obj.Image + "'>");
                    var name = $("<h3>" + obj.name + "</h3>");
                    var comment = $("<p>" + obj.commentstring + "</p>");
                    var container = $(".modal-body");
                    container.append(image);
                    container.append(name);
                    container.append(comment);


                }
            }

        },
        error: function () {
            console.log(res);
            alert("\Failed Operation");
        }
    }
    $.ajax(settings);
}
function AddtoFavourite(obj)
{
    var pid = $(obj).closest(".box").find("#ppid").text();
    var uid = $(obj).closest(".box").find("#userid").val();
    var data = { "Productid": pid, "Userid": uid };

    var settings = {
        type: "POST",
        dataType: "json",
        url: "/Favorite/Addtofavorit",
        data: data,
        success: function (res) {
           
            if (res.urli != "") {
                alert("Sucessfully added into favourite");
                location.href = res.urli;

            }
            else {
                alert("Errorr>>>>>>>>>>>>>>");
            }
        },
        error: function () {
            console.log(res);
            alert("\Failed Operation");
        }
    }
    $.ajax(settings);
   
}
function Rate(obj)
{
    var pid = $(obj).closest(".box").find("#ppid").text();
    var obtainedpoints = $(obj).val();
    var totalpoints = 5;
    var data = { "Productid": pid, "ObtainedPoints": obtainedpoints, "TotalPoints": totalpoints };
    var settings = {
        type: "POST",
        dataType: "json",
        url: "/Product/RateProduct",
        data: data,
        success: function (res) {

            if (res.lage !=false) {
                alert("Product rated successfully");

            }
            else {
                alert("Errorr>>>>>>>>>>>>>>");
            }
        },
        error: function () {
           
            alert("\Failed Operation");
        }
    }
    $.ajax(settings);
  
}
function SendEmail(obj)
{
    var email = prompt("Please enter your Friends email adress ");
            var pid = $(obj).closest(".box").find("#ppid").text();
            var data = { "txtEmail": email, "id": pid };
            var settings = {
                type: "POST",
                dataType: "json",
                url: "/Product/Email",
                data: data,
                success: function (res) {

                    if (res.flag != false) {
                        alert("Email successfully been sent");
                        location.href = res.urli;

                    }
                    else {
                        alert("Invalid Email address");
                        location.href = res.urli;
                    }
                },
                error: function () {

                    alert("\Failed Operation");
                    location.href = res.urli;
                }
            }
            $.ajax(settings);
        
   
}