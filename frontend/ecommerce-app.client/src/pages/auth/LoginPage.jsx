import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { login } from '../../services/authService';

export default function LoginPage() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await login(username, password);
      navigate('/wall');
    } catch (err) {
      setError(err.response?.data?.Message || 'Invalid username or password.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="my-12 bg-slate-900 border border-slate-800 p-8 rounded-xl">
      <h2 className="text-2xl font-bold text-slate-100 mb-6 text-center">Sign In</h2>
      
      {error && (
        <div className="bg-red-500/10 border border-red-500 text-red-400 text-sm p-3 rounded-lg mb-4">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block text-xs font-semibold text-slate-400 uppercase tracking-wider mb-2">Username</label>
          <input 
            type="text" 
            required
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full h-10 bg-slate-950 border border-slate-800 focus:border-teal-500 focus:outline-none px-4 text-slate-100 rounded-lg transition-colors"
          />
        </div>

        <div>
          <label className="block text-xs font-semibold text-slate-400 uppercase tracking-wider mb-2">Password</label>
          <input 
            type="password" 
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full h-10 bg-slate-950 border border-slate-800 focus:border-teal-500 focus:outline-none px-4 text-slate-100 rounded-lg transition-colors"
          />
        </div>

        <button 
          type="submit" 
          disabled={loading}
          className="w-full h-10 bg-teal-500 hover:bg-teal-400 disabled:bg-teal-800 text-slate-950 font-bold text-sm rounded-lg transition-colors"
        >
          {loading ? 'Signing In...' : 'Sign In'}
        </button>
      </form>

      <p className="text-sm text-slate-400 text-center mt-6">
        New to Ecomm? <Link to="/register" className="text-teal-400 hover:underline">Create an account</Link>
      </p>
    </div>
  );
}