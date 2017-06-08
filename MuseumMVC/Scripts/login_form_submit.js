$(document).delegate("#login_Form_Submit", "click", function () {
    Login();
});

function Login() {

    var _userName = $("#userName").val();
    var _password = $("#password").val();

    $.post('/Home/manuelTjekIndTjekUd', { userName: _userName, password: _password}, function (data) {
        var data = JSON.parse(data);
        if (data.code == 200) {
            console.log(data.msg);

            swal({
                title: data.msg + " OK",
                text: "",
                type: "success",
                timer: 3000,
                showConfirmButton: false
            });

            $("#userName").val('');
            $("#password").val('');

        } else {
            //ERROR - DO STUFF!
            console.log(data.msg);

            swal({
                title: data.msg,
                text: "",
                type: "error",
                timer: 3000,
                showConfirmButton: false
            });
        }
    })
}