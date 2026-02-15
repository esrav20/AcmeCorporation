document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById('draw-form');
    if (!form) return;

    const alertBox = document.getElementById('ajax-alert');
    const submitButton = document.getElementById('button[type="submit"]');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        clearErrors();

        const payload = {
            firstName: document.querySelector('[name="FirstName"]').value.trim(),
            lastName: document.querySelector('[name="LastName"]').value.trim(),
            email: document.querySelector('[name="Email"]').value.trim(),
            serialNumber: document.querySelector('[name="SerialNumber"]').value.trim(),
            dateOfBirth: document.querySelector('[name="DateOfBirth"]').value.trim()
        };

        submitBtn.disabled = true;
        submitBtn.textContent = 'Submitting...';

        try {
            const token = document.querySelector('[name="__RequestVerificationToken"]').value;

            const response = await fetch('/api/ApiDraw', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify(payload)
            });

            const data = await response.json();

            if (response.ok && data.success) {
                showAlert('success', data.message);
                form.reset();
            } else {
                showAlert('danger', data.errorMessage || 'Something went wrong.');
                if (data.errorField) {
                    highlightField(data.errorField);
                }
            }
        } catch (err) {
            showAlert('danger', 'Something went wrong.');
            console.error('AJAX submit error:', err);
        } finally {
            submitBtn.disabled = false;
            submitBtn.textContent = 'Submit Entry';
        }
    });

    function clearErrors() {
        if (alertBox) alertBox.classList.add('d-none');
        form.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
    }

    function showAlert(type, message) {
        if (!alertBox) return;
        alertBox.className = 'alert alert-${type}';
        alertBox.textContent = message;
        alertBox.classList.remove('d-none');
    }

    function highlightField(fieldName) {
        const input = form.querySelector('${fieldName}');
        if (input) input.classList.add('is-invalid');
    }
});