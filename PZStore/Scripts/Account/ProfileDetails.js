function changeLock(fieldId) {
    var element = document.getElementById(fieldId);

    if (element.getAttribute('readonly') === 'true') {
        element.removeAttribute("readonly");
    } else {
        element.setAttribute("readonly", true);
    }
}