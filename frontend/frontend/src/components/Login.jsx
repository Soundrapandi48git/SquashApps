import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { Input, Button, Card, Typography, message } from 'antd';

const LoginPage = ({ setAuth }) => {
    const [formData, setFormData] = useState({ email: '', password: '' });
    const navigate = useNavigate();

    const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const res = await axios.post('http://localhost:5281/api/account/login', formData);
            localStorage.setItem('token', res.data.token);
            setAuth(true);
            navigate('/dashboard');
        } catch (err) {
            message.error(err.response.data.msg || 'Login failed');
        }
    };

    return (
        <div className="flex justify-center items-center min-h-screen bg-gray-100 p-4">
            <Card className="w-full max-w-md shadow-xl p-6">
                <Typography.Title level={2} className="text-center mb-6">Login</Typography.Title>
                <form onSubmit={handleSubmit} className="space-y-6">
                    <div className="space-y-2">
                        <Input
                            name="email"
                            type="email"
                            placeholder="Email"
                            value={formData.email}
                            onChange={handleChange}
                            required
                            className="w-full py-2"
                        />
                    </div>
                    <div className="space-y-2">
                        <Input.Password
                            name="password"
                            placeholder="Password"
                            value={formData.password}
                            onChange={handleChange}
                            required
                            className="w-full py-2"
                        />
                    </div>
                    <Button
                        type="primary"
                        htmlType="submit"
                        className="w-full h-10 bg-blue-600 hover:bg-blue-700"
                    >
                        Login
                    </Button>
                </form>
                <div className="text-center mt-6">
                    <span className="text-gray-600">Don't have an account?</span>{' '}
                    <Link to="/register" className="text-blue-600 hover:text-blue-800 font-medium">
                        Register
                    </Link>
                </div>
            </Card>
        </div>
    );
};

export default LoginPage;
