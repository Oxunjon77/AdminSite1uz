
function previewImage(event) {
    var reader = new FileReader();

    reader.onload = function () {
        var output = document.getElementById('image-preview');
        output.src = reader.result;
        output.style.display = 'block'; 
    }

    reader.readAsDataURL(event.target.files[0]); 
}


function validateForm() {
    var title = document.getElementById('short-news').value.trim();
    var content = document.getElementById('detailed-news').value.trim();
    var image = document.getElementById('image').files.length;

    if (!title) {
        Swal.fire({
            icon: 'warning',
            title: 'Қисқача янгилик киритилмади',
            text: 'Илтимос, қисқача янгиликни киритинг.'
        });
        return false;
    }

    if (!content) {
        Swal.fire({
            icon: 'warning',
            title: 'Тўлиқ янгилик киритилмади',
            text: 'Илтимос, тўлиқ янгиликни киритинг.'
        });
        return false;
    }

    if (image === 0) {
        Swal.fire({
            icon: 'warning',
            title: 'Расм танланмади',
            text: 'Илтимос, расм танланг.'
        });
        return false;
    }


    Swal.fire({                                     
        title: 'Статус танланг',
        text: "Янгиликни қайси ҳолатда сақламоқчисиз?",
        icon: 'question',
        showCancelButton: true,
        showDenyButton: true,
        confirmButtonText: 'Асосий',
        denyButtonText: 'Черновик',
        cancelButtonText: 'Бекор қилиш'
    }).then((result) => {
        const form = document.querySelector("form");
        if (result.isConfirmed) {                                       
            document.getElementById("Status").value = 1;


            const formData = new FormData(form);
            fetch(form.action, {
                method: "POST",
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        window.location.href = '/News/Index';
                    } else {
                        Swal.fire("Хатолик!", "Маълумотларни сақлаб бўлмади", "error");
                    }
                });
        } else if (result.isDenied) {                                      
            document.getElementById("Status").value = 0;

            const formData = new FormData(form);
            fetch(form.action, {
                method: "POST",
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        window.location.href = '/Test_news/Index';
                    } else {
                        Swal.fire("Хатолик!", "Маълумотларни сақлаб бўлмади", "error");
                    }
                });
        }
    });
    return false;

}
