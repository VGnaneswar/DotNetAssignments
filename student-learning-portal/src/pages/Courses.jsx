import { useNavigate } from "react-router-dom";
import courses from "../data/courses";

function Courses() {
  const navigate = useNavigate();

  return (
    <div>
      <h1>Available Courses</h1>

      {courses.map((course) => (
        <div key={course.id} className="course-card">
          <h2>{course.title}</h2>
          <p>Category: {course.category}</p>
          <p>Duration: {course.duration}</p>
          <p>Trainer: {course.trainer}</p>

          <button
            onClick={() => navigate(`/courses/${course.id}`)}
          >
            View Details
          </button>

          <hr />
        </div>
      ))}
    </div>
  );
}

export default Courses;