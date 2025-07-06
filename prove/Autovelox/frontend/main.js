const baseUrl = 'https://localhost:7183/api/Comuni/GetComuni'

fetch(baseUrl, {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify("")
})
.then(response => {
  if (!response.ok) {
    throw new Error('Errore nella richiesta POST');
  }
  return response.json();
})
.then(data => {
  console.log(typeof data);
  console.log(data.$values);
  const tableBody = document.getElementById('resultsTable').querySelector('tbody');
  tableBody.innerHTML = ''; 

  data.$values.forEach(comune => {
    const row = document.createElement('tr');

    row.innerHTML = `
      <td>${comune.idComune}</td>
      <td>${comune.nome}</td>
      <td>${comune.provincia}</td>
      <td>${comune.regione}</td>
      <td>${comune.ripartizioneGeografica}</td>
    `;

    tableBody.appendChild(row);
  });
})
.catch(error => {
  console.error('Errore:', error);
});


document.getElementById('filterForm').addEventListener('submit', function (e) {
  e.preventDefault();

  const filterValue = document.getElementById('textFilter').value;

  fetch(baseUrl, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(filterValue)
  })
  .then(response => {
    if (!response.ok) {
      throw new Error('Errore nella richiesta POST');
    }
    return response.json();
  })
  .then(data => {
    const tableBody = document.getElementById('resultsTable').querySelector('tbody');
    tableBody.innerHTML = ''; // Svuota la tabella prima di riempirla

    data.$values.forEach(comune => {
      const row = document.createElement('tr');

      row.innerHTML = `
        <td>${comune.idComune}</td>
        <td>${comune.nome}</td>
        <td>${comune.provincia}</td>
        <td>${comune.regione}</td>
        <td>${comune.ripartizioneGeografica}</td>
      `;

      tableBody.appendChild(row);
    });
  })
  .catch(error => {
    console.error('Errore:', error);
  });
});

  