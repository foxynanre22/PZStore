function changeLock(fieldId, lockImageId, unlockImageId, buttonId) {
    var field = document.getElementById(fieldId);
    var lockImage = document.getElementById(lockImageId);
    var unlockImage = document.getElementById(unlockImageId);
    var button = document.getElementById(buttonId);

    if (field.getAttribute('readonly') === 'true') {
        field.removeAttribute("readonly");
        button.style.backgroundColor = '#fd6e6e';
        lockImage.style.visibility = 'hidden';
        unlockImage.style.visibility = 'visible';
    } else {
        field.setAttribute("readonly", true);
        button.style.backgroundColor = '#6B8E23';
        lockImage.style.visibility = 'visible';
        unlockImage.style.visibility = 'hidden';
    }
}