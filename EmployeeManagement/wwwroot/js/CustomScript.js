function ConfirmDelete(userId, isDeleteClicked) {
    var deleteSpan = $('#deleteUser_' + userId);
    var confirmSpan = $('#confirmDelete_' + userId);

    if (isDeleteClicked) {
        deleteSpan.hide();
        confirmSpan.show();
    }
    else {
        confirmSpan.hide();
        deleteSpan.show();
    }
}