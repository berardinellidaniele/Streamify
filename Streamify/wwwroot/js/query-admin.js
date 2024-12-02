document.addEventListener("DOMContentLoaded", function () {
    const queryButtons = document.querySelectorAll(".query-button");
    const query5Element = document.querySelector(".query-item.query-5");
    const allButtons = document.querySelectorAll(".query-button");

    queryButtons.forEach((button) => {
        button.addEventListener("click", () => {
            const queryContent = button.parentElement.nextElementSibling;

            if (query5Element.contains(button)) {
                if (query5Element.classList.contains("expanded")) {
                    query5Element.classList.remove("expanded");
                    queryContent.style.display = "none";
                    button.textContent = "+";

                    allButtons.forEach((btn) => btn.disabled = false);
                } else {
                    query5Element.classList.add("expanded");
                    queryContent.style.display = "block";
                    button.textContent = "-";

                    allButtons.forEach((btn) => {
                        if (!query5Element.contains(btn)) btn.disabled = true;
                    });
                }
            } else {
                if (queryContent.style.display === "block") {
                    queryContent.style.display = "none";
                    button.textContent = "+";
                } else {
                    queryContent.style.display = "block";
                    button.textContent = "-";
                }
            }
        });
    });
});
