/** @type {import('tailwindcss').Config} */
module.exports = {
    darkMode: "class",
    content: [
        "./Views/**/*.cshtml",
        "./wwwroot/js/**/*.js"
    ],
    theme: {
        extend: {
            colors: {
                "primary": "#003366",
                "background-light": "#f5f7f8",
                "background-dark": "#0f1923",
            },
            fontFamily: {
                "display": ["Inter"]
            },
            borderRadius: {
                "DEFAULT": "0.25rem",
                "lg": "0.5rem",
                "xl": "0.75rem",
                "full": "9999px"
            },
        },
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/container-queries')
    ],
}