import { useParams, useNavigate } from "react-router-dom";
import courses from "../data/courses";

function CourseDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const course = courses.find(
    (course) => course.id === Number(id)
  );

  if (!course) {
    return <h2>Course not found</h2>;
  }

  return (
    <div>
      <h1>Course Details</h1>

      <p>Course ID: {course.id}</p>
      <p>Title: {course.title}</p>
      <p>Category: {course.category}</p>
      <p>Duration: {course.duration}</p>
      <p>Trainer: {course.trainer}</p>
      <p>Description: {course.description}</p>

      <button onClick={() => navigate("/courses")}>
        Back to Courses
      </button>
    </div>
  );
}

export default CourseDetails;