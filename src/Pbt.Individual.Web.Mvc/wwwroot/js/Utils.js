function formatDateToDDMMYYYYHHmm(date) {
    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');   
    const year = d.getFullYear();
    const hours = String(d.getHours()).padStart(2, '0');
    const minutes = String(d.getMinutes()).padStart(2, '0');
    return `${day}/${month}/${year} ${hours}:${minutes}`;
}

function formatDateToYYYYMMDD(date) {
    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');   
    const year = d.getFullYear();
    return `${year}-${month}-${day}`;
}
    
function formatNumberWithThousandsSeparator(num) {
    return Number(num).toLocaleString('en-US', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 20
    });
}   