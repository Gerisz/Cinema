function checkSeats() {
    var seats = Array.from(document.getElementsByClassName('seat'));
    if (seats.filter(s => s.checked).length >= 6)
        seats.filter(s => !s.checked).forEach(s => s.disabled = true);
    else
        seats.forEach(s => s.disabled = false);
        
}