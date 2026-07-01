import { NavLink, Outlet } from "react-router-dom";

function Dashboard() {
  return (
    <div>
      <h1>Welcome to Student Dashboard</h1>

      <nav>
        <NavLink to="profile">Profile</NavLink> |{" "}
        <NavLink to="my-courses">My Courses</NavLink> |{" "}
        <NavLink to="settings">Settings</NavLink>
      </nav>

      <hr />

      <Outlet />
    </div>
  );
}

export default Dashboard;