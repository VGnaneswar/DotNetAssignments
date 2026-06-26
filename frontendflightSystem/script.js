const startingFlights = [
  { time: "12:40", flight: "AA 8826", dest: "Chicago", gate: "A11", status: "ON TIME" },
  { time: "12:50", flight: "F9 0970", dest: "London", gate: "C11", status: "BOARDING" },
  { time: "12:59", flight: "F9 1635", dest: "Boston", gate: "B05", status: "GATE CLOSED" },
  { time: "13:11", flight: "AS 3188", dest: "New York", gate: "D12", status: "GATE CLOSED" },
  { time: "13:37", flight: "BA 1760", dest: "San Francisco", gate: "B20", status: "DELAYED" },
  { time: "13:50", flight: "DL 2330", dest: "San Francisco", gate: "A04", status: "DEPARTED" },
  { time: "14:26", flight: "AC 0202", dest: "London", gate: "C14", status: "GATE CLOSED" },
  { time: "15:05", flight: "NH 0175", dest: "Tokyo", gate: "D02", status: "DEPARTED" },
  { time: "15:15", flight: "WN 0612", dest: "Las Vegas", gate: "B09", status: "DEPARTED" }
];

const statusTransitions = {
  "ON TIME": "BOARDING",
  BOARDING: "GATE CLOSED",
  "GATE CLOSED": "DEPARTED",
  DELAYED: "BOARDING",
  DEPARTED: "DEPARTED"
};

const destinations = ["Paris", "Rome", "Tokyo", "Seoul", "Doha", "Toronto", "Auckland", "Berlin", "Madrid", "Lisbon"];

const board = document.getElementById("board");
const clock = document.getElementById("clock");
const summary = document.getElementById("summary");
const addFlightButton = document.getElementById("addFlightButton");
const resetButton = document.getElementById("resetButton");

let flights = cloneFlights(startingFlights);
let nextFlightIndex = flights.length + 1;
let nextAutoUpdateIndex = 0;
let activeRows = new Map();

function cloneFlights(sourceFlights) {
  return sourceFlights.map((flight, index) => ({
    ...flight,
    id: index + 1
  }));
}

function getStatusClass(status) {
  return status.toLowerCase().replace(/\s+/g, "-");
}

function createBoardRow(flight) {
  const row = document.createElement("div");
  row.className = "board-row row-enter";
  row.dataset.flightId = String(flight.id);

  const timeCell = document.createElement("div");
  timeCell.className = "board-cell";
  timeCell.textContent = flight.time;

  const flightCell = document.createElement("div");
  flightCell.className = "board-cell";
  flightCell.textContent = flight.flight;

  const destCell = document.createElement("div");
  destCell.className = "board-cell";
  destCell.textContent = flight.dest;

  const gateCell = document.createElement("div");
  gateCell.className = "board-cell";
  gateCell.textContent = flight.gate;

  const statusCell = document.createElement("div");
  statusCell.className = `board-cell status-cell ${getStatusClass(flight.status)}`;
  statusCell.textContent = flight.status;
  statusCell.dataset.statusCell = "true";

  row.appendChild(timeCell);
  row.appendChild(flightCell);
  row.appendChild(destCell);
  row.appendChild(gateCell);
  row.appendChild(statusCell);

  return row;
}

function renderBoard() {
  board.replaceChildren();
  activeRows = new Map();

  flights.forEach((flight) => {
    const row = createBoardRow(flight);
    activeRows.set(flight.id, row);
    board.appendChild(row);
  });

  updateSummary();
}

function updateRowStatus(flightId, nextStatus) {
  const flight = flights.find((entry) => entry.id === flightId);
  if (!flight) {
    return;
  }

  flight.status = nextStatus;

  const row = activeRows.get(flightId);
  if (!row) {
    return;
  }

  const statusCell = row.querySelector("[data-status-cell='true']");
  if (!statusCell) {
    return;
  }

  statusCell.textContent = nextStatus;
  statusCell.className = `board-cell status-cell ${getStatusClass(nextStatus)} status-flash`;

  window.setTimeout(() => {
    statusCell.className = `board-cell status-cell ${getStatusClass(nextStatus)}`;
  }, 450);

  updateSummary();
}

function advanceFlightStatuses() {
  const candidates = flights.filter((flight) => flight.status !== "DEPARTED");
  if (!candidates.length) {
    return;
  }

  const flight = candidates[nextAutoUpdateIndex % candidates.length];
  nextAutoUpdateIndex += 1;

  const nextStatus = statusTransitions[flight.status];
  if (nextStatus && nextStatus !== flight.status) {
    updateRowStatus(flight.id, nextStatus);
  }
}

function addDeparture() {
  const hour = 16 + Math.floor(Math.random() * 6);
  const minute = String(Math.floor(Math.random() * 60)).padStart(2, "0");
  const gatePrefix = ["A", "B", "C", "D", "E"][(nextFlightIndex - 1) % 5];

  flights = [
    ...flights,
    {
      id: nextFlightIndex,
      time: `${String(hour).padStart(2, "0")}:${minute}`,
      flight: `FF ${String(4000 + nextFlightIndex).slice(-4)}`,
      dest: destinations[(nextFlightIndex - 1) % destinations.length],
      gate: `${gatePrefix}${String(((nextFlightIndex - 1) % 14) + 1).padStart(2, "0")}`,
      status: "ON TIME"
    }
  ];

  nextFlightIndex += 1;

  const newFlight = flights[flights.length - 1];
  const row = createBoardRow(newFlight);
  activeRows.set(newFlight.id, row);
  board.appendChild(row);

  updateSummary();
}

function resetBoard() {
  flights = cloneFlights(startingFlights);
  nextFlightIndex = flights.length + 1;
  nextAutoUpdateIndex = 0;
  renderBoard();
}

function updateClock() {
  clock.textContent = new Intl.DateTimeFormat("en-US", {
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
    hour12: false
  }).format(new Date());
}

function updateSummary() {
  const total = flights.length;
  const boarding = flights.filter((flight) => flight.status === "BOARDING").length;
  const delayed = flights.filter((flight) => flight.status === "DELAYED").length;

  summary.textContent = `${total} departures · ${boarding} boarding · ${delayed} delayed`;
}

function pickRandomStatusChange() {
  const activeFlights = flights.filter((flight) => flight.status !== "DEPARTED");
  if (!activeFlights.length) {
    return;
  }

  const flight = activeFlights[Math.floor(Math.random() * activeFlights.length)];
  const nextStatus = statusTransitions[flight.status];

  if (nextStatus && nextStatus !== flight.status) {
    updateRowStatus(flight.id, nextStatus);
  }
}

addFlightButton.addEventListener("click", addDeparture);
resetButton.addEventListener("click", resetBoard);

renderBoard();
updateClock();
updateSummary();

window.setInterval(updateClock, 1000);
window.setInterval(advanceFlightStatuses, 4000);
window.setInterval(pickRandomStatusChange, 9000);
