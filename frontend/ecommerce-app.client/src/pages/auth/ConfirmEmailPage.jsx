import { useEffect, useState } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import { confirmEmail } from '../../services/authService';

export default function ConfirmEmailPage() {
  const [searchParams] = useSearchParams();
  const [status, setStatus] = useState('verifying'); // 'verifying', 'success', 'error'
  const [message, setMessage] = useState('');

  useEffect(() => {
    const token = searchParams.get('token');

    if (!token) {
      setStatus('error');
      setMessage('Activation token is missing.');
      return;
    }

    confirmEmail(token)
      .then(() => {
        setStatus('success');
      })
      .catch((err) => {
        setStatus('error');
        setMessage(err.response?.data?.Message || 'The token is invalid or has expired.');
      });
  }, [searchParams]);

  return (
    <div className="my-16 bg-slate-900 border border-slate-800 p-8 rounded-xl text-center">

      {status === 'verifying' && (
        <>
          <div className="animate-spin inline-block w-8 h-8 border-4 border-teal-500 border-t-transparent rounded-full mb-4"></div>
          <h2 className="text-xl font-bold text-slate-100">Verifying your email...</h2>
          <p className="text-slate-400 text-sm mt-2">Please wait, we are validating your token.</p>
        </>
      )}

      {status === 'success' && (
        <>
          <div className="text-4xl mb-4">✅</div>
          <h2 className="text-2xl font-bold text-slate-100 mb-4">Email Confirmed!</h2>
          <p className="text-slate-400 text-sm mb-6">
            Your account is now fully activated. You can safely log into the system.
          </p>
          <Link to="/login" className="inline-block px-6 py-2 bg-teal-500 text-slate-950 text-sm font-bold rounded-lg hover:bg-teal-400 transition-colors">
            Sign In
          </Link>
        </>
      )}

      {status === 'error' && (
        <>
          <div className="text-4xl mb-4">❌</div>
          <h2 className="text-2xl font-bold text-red-400 mb-4">Verification Failed</h2>
          <p className="text-slate-400 text-sm mb-6">{message}</p>
          <Link to="/register" className="inline-block px-6 py-2 bg-slate-950 border border-slate-800 text-sm font-semibold text-teal-400 rounded-lg hover:bg-slate-800 transition-colors">
            Back to Registration
          </Link>
        </>
      )}
      
    </div>
  );
}
