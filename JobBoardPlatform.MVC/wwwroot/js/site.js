(() => {
    "use strict";

    const flashMessages = document.querySelectorAll(".flash-message");
    const dismissFlash = (element) => {
        if (!element || element.classList.contains("is-hiding")) return;
        element.classList.add("is-hiding");
        window.setTimeout(() => element.remove(), 230);
    };

    flashMessages.forEach((message) => {
        message.querySelector("[data-flash-close]")?.addEventListener("click", () => dismissFlash(message));
        window.setTimeout(() => dismissFlash(message), 6000);
    });

    document.querySelectorAll("[data-password-toggle]").forEach((button) => {
        button.addEventListener("click", () => {
            const inputId = button.getAttribute("data-password-toggle");
            const input = inputId ? document.getElementById(inputId) : null;
            if (!input) return;

            const shouldShow = input.type === "password";
            input.type = shouldShow ? "text" : "password";
            button.setAttribute("aria-label", shouldShow ? "Hide password" : "Show password");
            const icon = button.querySelector("i");
            icon?.classList.toggle("fa-eye", !shouldShow);
            icon?.classList.toggle("fa-eye-slash", shouldShow);
        });
    });

    const advancedToggle = document.querySelector("[data-advanced-toggle]");
    const advancedPanel = document.querySelector("[data-advanced-panel]");
    advancedToggle?.addEventListener("click", () => {
        const isOpen = advancedPanel?.classList.toggle("is-open") ?? false;
        advancedToggle.setAttribute("aria-expanded", String(isOpen));
        const text = advancedToggle.querySelector("span");
        if (text) text.textContent = isOpen ? "Hide advanced filters" : "More filters";
    });

    const sidebar = document.getElementById("adminSidebar");
    const overlay = document.querySelector(".admin-overlay");
    document.querySelectorAll("[data-admin-sidebar]").forEach((button) => {
        button.addEventListener("click", () => {
            const isOpen = !sidebar?.classList.contains("is-open");
            sidebar?.classList.toggle("is-open", isOpen);
            overlay?.classList.toggle("is-open", isOpen);
            document.body.style.overflow = isOpen ? "hidden" : "";
        });
    });

    document.addEventListener("keydown", (event) => {
        if (event.key === "Escape" && sidebar?.classList.contains("is-open")) {
            sidebar.classList.remove("is-open");
            overlay?.classList.remove("is-open");
            document.body.style.overflow = "";
        }
    });

    document.querySelectorAll("[data-file-input]").forEach((input) => {
        input.addEventListener("change", () => {
            const targetId = input.getAttribute("data-file-input");
            const target = targetId ? document.getElementById(targetId) : null;
            if (target && input.files?.length) target.textContent = input.files[0].name;
        });
    });

    document.querySelectorAll("[data-confirm]").forEach((form) => {
        form.addEventListener("submit", (event) => {
            const message = form.getAttribute("data-confirm") || "Are you sure you want to continue?";
            if (!window.confirm(message)) event.preventDefault();
        });
    });
})();
