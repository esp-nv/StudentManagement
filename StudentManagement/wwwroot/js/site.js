// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const firstNameInput = document.querySelector('input[name="FirstName"]');
const firstNameHint = document.querySelector('.first-name-hint');

if (firstNameInput && firstNameHint) {
    const maxLength = firstNameInput.maxLength;

    firstNameInput.addEventListener('input', function () {
        const remainingCharacters = maxLength - this.value.length;

        firstNameHint.textContent =
            `First name → remaining ${remainingCharacters} characters.`;
    });
}
