// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Character counter for form fields.
// The hint is visible only while the field is active.

document.querySelectorAll('[data-character-counter]').forEach(input => {
    const hint = input
        .closest('.mb-3')
        ?.querySelector('.character-hint');

    if (!hint) {
        return;
    }

    input.addEventListener('focus', function () {
        updateCharacterHint(this, hint);
        hint.style.display = 'block';
    });

    input.addEventListener('input', function () {
        updateCharacterHint(this, hint);
    });

    input.addEventListener('blur', function () {
        hint.style.display = 'none';
    });

    hint.style.display = 'none';
});

function updateCharacterHint(input, hint) {
    const remainingCharacters =
        input.maxLength - input.value.length;

    hint.textContent =
        `${input.dataset.characterLabel} → remaining ${remainingCharacters} characters.`;
}
