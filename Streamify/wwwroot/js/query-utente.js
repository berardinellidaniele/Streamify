document.addEventListener("DOMContentLoaded", function () {
    const queryButtons = document.querySelectorAll(".query-button");
    const queryItems = document.querySelectorAll(".query-item");

    queryButtons.forEach((button) => {
        button.addEventListener("click", () => {
            const queryContent = button.parentElement.nextElementSibling;
            const currentQueryItem = button.closest(".query-item");

            if (queryContent.style.display === "block") {
                queryContent.style.display = "none";
                button.textContent = "+";
                currentQueryItem.classList.remove("expanded");

                queryItems.forEach((item) => {
                    if (item !== currentQueryItem) {
                        item.style.display = "block";
                    }
                });
            } else {
                queryItems.forEach((item) => {
                    if (item !== currentQueryItem) {
                        item.style.display = "none";
                    }
                });

                queryContent.style.display = "block";
                button.textContent = "-";
                currentQueryItem.classList.add("expanded");
            }
        });
    });
});
