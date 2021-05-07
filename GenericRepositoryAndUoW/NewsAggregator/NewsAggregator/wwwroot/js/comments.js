let commentsDisplaySwitcherElement = document.getElementById('comments-display-switcher');
let isCommentsDisplayed = false;

function toggleComments(newsId)
{
    //let url = window.location.pathname;
    //let id = url.substring(url.lastIndexOf('/') + 1);

    console.log(newsId);
    if (commentsDisplaySwitcherElement != null) {
        if (isCommentsDisplayed == true) {
            commentsDisplaySwitcherElement.innerHTML = 'Display comments';
            document.getElementById('comments-container').innerHTML = '';
        } else {
            commentsDisplaySwitcherElement.innerHTML = 'Hide comments';
            let commentsContainer = document.getElementById('comments-container');


            loadComments(newsId, commentsContainer);

        }
        isCommentsDisplayed = !isCommentsDisplayed;
    }
   
}

function loadComments(newsId, commentsContainer) {
    let request = new XMLHttpRequest();
    request.open('GET', `/Comments/List?newsId=${newsId}`, true);

    request.onload = function() {
        if (request.status >= 200 && request.status < 400) {
            let resp = request.responseText;
            commentsContainer.innerHTML = resp;
        }
    }

    request.send();
}
//document.onmousemove = function (e) {
//    let mousecoords = getMousePos(e);
//    console.log(`x = ${mousecoords.x} y =${mousecoords.y}`);
//};
//function getMousePos(e) {
//    return { x: e.clientX, y: e.clientY };
//}

//commentsDisplaySwitcherElement.onmouseover = function () {
//    commentsDisplaySwitcherElement.className = commentsDisplaySwitcherElement.className.replace("btn-primary", "btn-info");
//}
//commentsDisplaySwitcherElement.onmouseout = function () {
//    commentsDisplaySwitcherElement.className = commentsDisplaySwitcherElement.className.replace("btn-info", "btn-primary");
//}
/*
 * Mouse events
 * click
 * contextmenu
 * mouseover/mouseout
 * mousedown / mouseup
 * mousemove
 *
 * Form control events
 * submit
 * change
 * focus
 *
 * Keyboard events
 * keydown / keyup
 *
 * Document events
 * DOMContentLoaded 
 */