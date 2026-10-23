import { createBrowserRouter, RouterProvider, Navigate } from 'react-router-dom'
import MainLayout from './components/MainLayout'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import ConfirmEmailPage from './pages/ConfirmEmailPage'

const router = createBrowserRouter([
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {
        path: '/',
        element: <Navigate to="/wall" replace />,
      },
      {
        path: '/login',
        element: <LoginPage />,
      },
      {
        path: '/register',
        element: <RegisterPage />,
      },
      {
        path: '/confirm-email',
        element: <ConfirmEmailPage />,
      },
      {
        path: '/wall',
        element: (
          <div className="max-w-4xl mx-auto">
            <h1 className="text-3xl font-bold text-slate-100 mb-2">Wall</h1>
            <p className="text-slate-400 text-sm">WallPosts coming soon.</p>
          </div>
        )
      }
    ]
  },
  {
    path: '*',
    element: <Navigate to="/wall" replace />, 
  },
]);

export default function App() {
  return <RouterProvider router={router} />;
}