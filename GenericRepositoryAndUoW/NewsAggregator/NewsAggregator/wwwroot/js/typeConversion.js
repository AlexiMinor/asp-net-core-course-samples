//tostring =>

let a = 1;
let b = '11';
console.log(b-a);
let stringA = String(a);

//number
console.log('NUMBER');
let x = '1.5';
let y = 2;
console.log(x+y);//->12
console.log(Number(x) + Number(y));
console.log(Number("as"));

console.log(Number('   157   '));
console.log(Number(true));
console.log(Number(false));
console.log(Number(NaN));
console.log(Number(null));
console.log(Number(undefined));


//boolean
console.log('BOOLEAN');
console.log(Boolean('true'));
console.log(Boolean(''));
console.log(Boolean(' '));
console.log(Boolean('a'));
console.log(Boolean(1));
console.log(Boolean(2));
console.log(Boolean(0));
console.log(Boolean(-1));

// 0, null, undefined, NaN, "" - false 
// any other - true