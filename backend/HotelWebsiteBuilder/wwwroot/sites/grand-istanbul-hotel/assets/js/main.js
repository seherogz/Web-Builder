function toggleCollapse(contentId) {
    var contentDiv = document.getElementById(contentId);
    var iconSpan = document.querySelector("[data-content='" + contentId + "']");
    var headDiv = iconSpan.closest('.faq-collapse-head');

    if (contentDiv.style.display === "none") {
        contentDiv.style.display = "block";
        iconSpan.innerHTML = '<i class="fas fa-chevron-up"></i>';
        headDiv.classList.add('active');
    } else {
        contentDiv.style.display = "none";
        iconSpan.innerHTML = '<i class="fas fa-chevron-down"></i>';
        headDiv.classList.remove('active');
    }
}

function adjustFontSize(area) {
    const titleElements = document.querySelectorAll(area)
    let maxHeight = 1024;
    const fontSizeStep = 1;
    titleElements.forEach(titleElement => {
        const titleHeight = titleElement.scrollHeight;
        maxHeight = Math.min(maxHeight, titleHeight);
    })
    titleElements.forEach(titleElement => {
        let fontSize = 16;

        while (true) {
            if (maxHeight < titleElement.offsetHeight) {
                fontSize -= fontSizeStep;
                titleElement.style.fontSize = fontSize + 'px';
            } else {
                break;
            }
        }
    });

}

window.addEventListener('load', function () {
    adjustFontSize(".mounts-item-title");
});
window.addEventListener('resize', function () {
    adjustFontSize(".mounts-item-title")
});

document.querySelectorAll("form[ajax]").forEach(form => {

    form.addEventListener("submit", (event) => {

        let action = form.getAttribute("action");

        if (typeof action !== "undefined") {

            const formData = new FormData(form);

            fetch("system/ajax.php?islem=" + action, {
                method: "POST",
                body: formData
            })
                .then(response => response.json())
                .then(response => {

                    if (typeof response.refresh !== "undefined") {
                        if (typeof response.time !== "undefined") {
                            setTimeout(function () {
                                location.reload();
                            }, response.time)
                        } else {
                            location.reload();
                        }
                    }
                    if (typeof response.success !== "undefined") {
                        Swal.fire({
                            icon: 'success',
                            text: response.success,
                        })
                    } else if (typeof response.error !== "undefined") {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: response.error,
                        })
                    }

                })
                .catch(error => {
                    console.error(error);
                });

        }

        event.preventDefault();
    })

})

document.querySelector(".mobile-bars").addEventListener("click", function (e) {

    document.querySelector(".mobile-menu").classList.toggle("active")
    document.querySelector("body").classList.toggle("mobile-menu-active")

    e.preventDefault()
})

document.querySelector(".mobile-menu .menu").innerHTML = document.querySelector("header .container .header-menu .menu").innerHTML;
