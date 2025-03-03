document.addEventListener("DOMContentLoaded", function () {
    fetch("/Reportes/ObtenerMetricas")
        .then(response => response.json())
        .then(data => {
            document.getElementById("totalInventario").textContent = data.totalInventario;
            document.getElementById("totalSalidas").textContent = data.totalSalidas;
            document.getElementById("totalSolicitudes").textContent = data.totalSolicitudes;
        })
        .catch(error => console.error("Error al cargar métricas:", error));
});
