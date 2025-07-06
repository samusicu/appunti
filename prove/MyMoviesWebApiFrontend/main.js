fetch("https://localhost:7157/api/Movies",{
    method : 'GET',
    headers: {
    'Content-Type': 'application/json'
  }
}).then(response =>{
    if(!response.ok)
        throw new Error('Errore nella richiesta GET');
    return response.json();
}).then(data => {
    console.log(data);
    const tableBody = document.getElementById('resultsTable').querySelector('tbody');
    tableBody.innerHTML = ''; 

    data.forEach(movie => {
    const row = document.createElement('tr');

    row.innerHTML = `
      <td>${movie.title}</td>
      <td>${movie.year}</td>
      <td>${movie.genres.map(g => g.name).join(", ")}</td>
    `;

    tableBody.appendChild(row);
  });
})
.catch(error => {
  console.error('Errore:', error);
});

