import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { Input, Button, Card, Typography, message } from 'antd';

const RegisterPage = ({ setAuth }) => {
    const [formData, setFormData] = useState({
        username: '',
        email: '',
        password: '',
        accountNumber: '',
        phoneNumber: ''
    });
    const navigate = useNavigate();

    // Generate a random 12-digit account number
    const generateAccountNumber = () => {
        let result = '';
        for (let i = 0; i < 12; i++) {
            result += Math.floor(Math.random() * 10);
        }
        return result;
    };

    // Set the account number when component mounts
    useEffect(() => {
        setFormData(prevData => ({
            ...prevData,
            accountNumber: generateAccountNumber()
        }));
    }, []);

    const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const { ...submitData } = formData;
            const res = await axios.post('http://localhost:5281/api/account/createaccountdetails', {
                userId: 0, // Assuming a static userId for now
                username: submitData.username,
                password: submitData.password,
                accountNumber: submitData.accountNumber,
                email: submitData.email,
                phoneNumber: submitData.phoneNumber,
                registeredOn: new Date().toISOString(),
                isActive: true,
                balance: 0
            });

            localStorage.setItem('token', res.data.token);
            setAuth(true);
            navigate('/dashboard');
        } catch (err) {
            message.error(err.response?.data?.msg || 'Registration failed');
        }
    };

    return (
        <div className="flex justify-center items-center min-h-screen bg-gray-100 p-4">
            <Card className="w-full max-w-md shadow-xl rounded-lg overflow-hidden">
                <Typography.Title level={2} className="text-center mb-6">Register</Typography.Title>
                <form onSubmit={handleSubmit} className="space-y-6">
                    {/* Form fields */}
                    <div className="space-y-2">
                        <Input
                            name="username"
                            placeholder="Username"
                            value={formData.username}
                            onChange={handleChange}
                            required
                            className="w-full px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                    </div>
                    <div className="space-y-2">
                        <Input
                            name="email"
                            type="email"
                            placeholder="Email"
                            value={formData.email}
                            onChange={handleChange}
                            required
                            className="w-full px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                    </div>
                    <div className='space-y-2'>
                        <Input
                            name="phoneNumber"
                            placeholder="Enter Your Mobile Number"
                            value={formData.phoneNumber}
                            onChange={handleChange}
                            required
                            className="w-full px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                    </div>
                    <div className="space-y-2">
                        <Input.Password
                            name="password"
                            placeholder="Password"
                            value={formData.password}
                            onChange={handleChange}
                            required
                            className="w-full px-4 py-2 rounded-md border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500"
                        />
                    </div>
                    <Button
                        type="primary"
                        htmlType="submit"
                        className="w-full py-2 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-md transition-colors"
                    >
                        Register
                    </Button>
                </form>
                <div className="text-center mt-6 text-gray-600">
                    Already have an account? <Link to="/login" className="text-blue-600 hover:text-blue-800 font-medium">Login</Link>
                </div>
            </Card>
        </div>
    );
};

export default RegisterPage;
