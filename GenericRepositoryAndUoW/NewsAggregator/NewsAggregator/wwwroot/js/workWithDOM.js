let element = document.getElementById('header-text');
console.log(element);

let listItems = document.querySelectorAll('ul > li');
console.log(listItems);

let itemsByClass = document.getElementsByClassName('container');
console.log(listItems);

let email = document.getElementsByName('Email')[0];
console.log(email);
let pswd = document.getElementsByName('Password')[0];
console.log(pswd);

email.value = "b.baggins@gmail.com";


function aggregateForm() {
    console.log(email.value);
    console.log(pswd.value);
}