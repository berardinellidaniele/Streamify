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
            } else {
                queryItems.forEach((item) => {
                    const content = item.querySelector(".query-content");
                    const itemButton = item.querySelector(".query-button");

                    content.style.display = "none";
                    itemButton.textContent = "+";
                    item.classList.remove("expanded"); 
                });

                queryContent.style.display = "block";
                button.textContent = "-";

                if (currentQueryItem.classList.contains("query-5")) {
                    currentQueryItem.classList.add("expanded");
                }
            }
        });
    });
});
