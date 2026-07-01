import { NavLink, useNavigate } from "react-router-dom";

function Navbar() {
  const navigate = useNavigate();

  const isLoggedIn = localStorage.getItem("isLoggedIn");

  const handleLogout = () => {
    localStorage.removeItem("isLoggedIn");
    navigate("/login");
  };

  return (
    <nav>
      <NavLink to="/">Home</NavLink> |{" "}
      <NavLink
        to="/about"
        className={({ isActive }) => (isActive ? "active" : "")}
        >
        About
        </NavLink>|{" "}
      <NavLink to="/courses">Courses</NavLink> |{" "}
      <NavLink to="/contact">Contact</NavLink> |{" "}
      
      {!isLoggedIn ? (
        <NavLink to="/login">Login</NavLink>) : (
        <>
          <NavLink to="/dashboard">Dashboard</NavLink> |{" "}
          <button onClick={handleLogout}>Logout</button>
        </>
      )}
    </nav>
  );
}

export default Navbar;