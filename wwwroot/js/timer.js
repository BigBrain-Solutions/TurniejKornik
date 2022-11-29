let a = setInterval(endOfEntries = () => {
    const date = new Date("Dec 2, 2022 00:00:00");
    const now = new Date().getTime();
    const dif = date - now;

    let days = Math.floor(dif / (1000 * 60 * 60 * 24));
    let hours = Math.floor((dif % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    let minutes = Math.floor((dif % (1000 * 60 * 60)) / (1000 * 60));
    let seconds = Math.floor((dif % (1000 * 60)) / 1000);

    formatTheTimer(days, hours, minutes, seconds, "endOfEntries");
    
    if (dif < 0) {
        clearInterval(x);
        document.getElementById("endOfEntries").innerHTML = "";
    }
}, 1);

let x = setInterval(startOfTournament = () => {
    const date = new Date("Dec 8, 2022 07:00:00");
    const now = new Date().getTime();
    const dif = date - now;

    let days = Math.floor(dif / (1000 * 60 * 60 * 24));
    let hours = Math.floor((dif % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    let minutes = Math.floor((dif % (1000 * 60 * 60)) / (1000 * 60));
    let seconds = Math.floor((dif % (1000 * 60)) / 1000);

    formatTheTimer(days, hours, minutes, seconds, "startOfTournament");

    if (dif < 0) {
        clearInterval(x);
        document.getElementById("startOfTournament").innerHTML = "";
    }
}, 1);

const formatTheTimer = (days, hours, minutes, seconds, id) => {
    let e = document.getElementById(id);
    
    if (days > 1) {
        e.innerHTML = days + " dni " + hours + " godzin ";
    }
    
    if (days < 1) {
        e.innerHTML = hours + " godzin "
            + minutes + " minut ";
        
        e.classList.remove("text-secondary");
        e.classList.add("text-danger");
    }
    
    if (hours < 1) {
        e.innerHTML = minutes + " minut " + seconds + " sekund ";
        
        e.classList.remove("text-secondary");
        e.classList.add("text-danger");
    }
    
    if (minutes < 1) {
        e.innerHTML = seconds + " sekund ";

        e.classList.remove("text-secondary");
        e.classList.add("text-danger");
    }
    
    return e;
}