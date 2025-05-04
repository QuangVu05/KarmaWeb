function showRegisterForm() {
    var loginForm = document.getElementById('login-form');
    var registerForm = document.getElementById('register-form');
    var toggleButton = document.getElementById('toggleButton');

    // Ẩn form login và hiện form register
    if (registerForm.style.display === 'none') {
        registerForm.style.display = 'block';
        loginForm.style.display = 'none';
        toggleButton.innerText = 'Login'; // Thay đổi tên nút thành "Login"
    } else {
        registerForm.style.display = 'none';
        loginForm.style.display = 'block';
        toggleButton.innerText = 'Create an Account'; // Thay đổi tên nút thành "Create an Account"
    }
}