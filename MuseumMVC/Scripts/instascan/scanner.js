var camList = [];
var scanner = new Instascan.Scanner({ video: document.getElementById('preview') });

function makeList(cameras) {
    if (cameras.length <= 1) {
        $("#cam_list").remove();
        return;
    }
    var _html = '';
    for (var i = 0; i < cameras.length; i++) {
        _html += `<option class="cam_list_item" data-index="${i}" value="${i}" style="cursor: pointer">${cameras[i].name}</option>`;
    }
    document.getElementById("cam_list").innerHTML = _html;
}

scanner.addListener('scan', function (content) {
    console.log(content);
    // Post send vvvv
    $.post('/Home/validateChekIn', { id: content }, function (data) {
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
    });
});
Instascan.Camera.getCameras().then(function (cameras) {
    if (cameras.length > 0) {
        scanner.start(cameras[0]);
        camList = cameras;
        makeList(camList);
    } else {
        console.error('No cameras found.');
    }
}).catch(function (e) {
    console.error(e);
});

function changeCam(indexNumber) {
    scanner.stop();
    Instascan.Camera.getCameras().then(function (cameras) {
        if (cameras.length > 0) {
            scanner.start(cameras[indexNumber]);
        } else {
            console.error('No cameras found.');
        }
    }).catch(function (e) {
        console.error(e);
    });
}
$("#cam_list").change(function () {
    var elm = $(this);
    var camIndex = parseInt(elm.val());
    changeCam(camIndex);
});