let userName = 'Vasya';
let Hello = 'Hello';

function showMessage() {
    console.log('Show message function from function');
        let messageText = Hello + ' ' + userName;
    alert(messageText);
}

function showMessage2(helloText = "Hello", userNameText = "User") {
    //userNameText = userNameText || "User";
    //if (userNameText === undefined) {
    //    userNameText = "User";
    //}
    console.log('Show message function with let');
    let messageText = helloText + ' ' + userNameText;
    alert(messageText);
}

function calculateSum(a,b) {
    return a + b;
}

function checkAge(age) {
    if (age > 18) {
        return true;
    } else {
        return confirm('Do you have agreement from parent?');
    }
}

function differentValuesInReturn(parameter) {
    if (!isNaN(parameter)) {
        return 1;
    } else {
        return "abc";
    }
}

function doSmth(parameter) {
    console.log(parameter);
}

function doSmth(parameter, parameter2) {
    console.log(parameter);
    console.log(parameter2);
}

//let doSmth2 = function(parameter) {
//    console.log(parameter);
//};

//doSmth2 = function (parameter, parameter2) {
//    console.log(parameter);
//    console.log(parameter2);
//}