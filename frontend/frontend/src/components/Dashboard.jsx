/* eslint-disable react-hooks/exhaustive-deps */
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { Card, InputNumber, Select, Button, Typography, message, Table } from 'antd';

const DashboardPage = () => {
    const navigate = useNavigate();
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const [amount, setAmount] = useState(0);
    const [type, setType] = useState('deposit');
    const [transactions, setTransactions] = useState([]);

    console.log('DashboardPage rendered', transactions);

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const token = localStorage.getItem('token');
                const config = { headers: { 'Authorization': token } };
                // const res = await axios.get(`http://localhost:5281/api/transaction/getspecifictransactiondetails/${user}`, config); // Replace with correct user info endpoint
                // setUser(res.data);

                const txnRes = await axios.get('http://localhost:5281/api/transaction/getalltransactiondetails', config);
                setTransactions(txnRes.data);
            } catch (err) {
                console.log(err);
                localStorage.removeItem('token');
                navigate('/login');
            } finally {
                setLoading(false);
            }
        };
        fetchUser();
    }, [navigate]);

    const handleTransaction = async () => {
        console.log('handleTransaction called', amount, type);
        if (!amount || amount <= 0) return message.error('Enter valid amount');

        try {
            const token = localStorage.getItem('token');
            const config = { headers: { 'Authorization': token } };
            const res = await axios.post('http://localhost:5281/api/transaction/createtransactiondetails', {
                userID: user.userId,
                accountNumber: user.accountNumber,
                amount,
                transactionType: type,
                description: `${type} transaction`
            }, config);

            setUser((prev) => ({ ...prev, balance: res.data.balance }));
            setAmount(0);

            const txnRes = await axios.get('http://localhost:5281/api/transaction/getalltransactiondetails', config);
            setTransactions(txnRes.data);
            message.success(`${type === 'deposit' ? 'Deposited' : 'Withdrew'} $${amount}`);
        } catch (err) {
            message.error(err.response?.data.msg || 'Transaction failed');
        }
    };

    const logout = () => {
        localStorage.removeItem('token');
        navigate('/login');
    };

    if (loading) return <div className="flex justify-center items-center mt-10">Loading...</div>;

    return (
        <div className="p-6 max-w-6xl mx-auto">
            <div className="flex justify-between items-center mb-6">
                <Typography.Title level={3}>Welcome, {user?.username}</Typography.Title>
                <Button danger onClick={logout}>Logout</Button>
            </div>

            <Card className="mb-6 shadow-md rounded-lg">
                <Typography.Title level={4}>Account Number: {user?.accountNumber}</Typography.Title>
                <Typography.Title level={4}>Balance: ₹{user?.balance.toFixed(2)}</Typography.Title>
                <div className="flex flex-wrap items-center gap-4 mt-4">
                    <Select
                        value={type}
                        onChange={(v) => setType(v)}
                        options={[
                            { label: 'Deposit', value: 'deposit' },
                            { label: 'Withdraw', value: 'withdraw' }
                        ]}
                        className="min-w-[120px]"
                    />
                    <InputNumber
                        value={amount}
                        onChange={setAmount}
                        min={0.01}
                        step={0.01}
                        className="min-w-[150px]"
                    />
                    <Button type="primary" onClick={handleTransaction} className="ml-auto sm:ml-0">
                        Submit
                    </Button>
                </div>
            </Card>

            <Card title="Transaction History" className="shadow-md rounded-lg">
                <Table
                    dataSource={transactions.map((txn, idx) => ({ ...txn, key: idx }))}
                    columns={[
                        {
                            title: 'Date', dataIndex: 'createdAt', key: 'date', render: (createdAt) => {
                                const date = new Date(createdAt);
                                const pad = (num) => String(num).padStart(2, '0');
                                return (
                                    <span>
                                        {`${pad(date.getDate())}/${pad(date.getMonth() + 1)}/${date.getFullYear()}  ${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`}
                                    </span>
                                );
                            }
                        },
                        {
                            title: 'Type', dataIndex: 'transactionType', key: 'type', render: (type) => (
                                <span className={`capitalize ${type === 'deposit' ? 'text-green-600' : 'text-red-600'}`}>
                                    {type}
                                </span>
                            )
                        },
                        {
                            title: 'Amount',
                            dataIndex: 'amount',
                            key: 'amount',
                            render: (amount, record) => (
                                <span className={record.transactionType === 'withdraw' ? 'text-red-500' : 'text-green-500'}>
                                    ₹{amount.toFixed(2)}
                                </span>
                            )
                        },
                    ]}
                />
            </Card>
        </div>
    );
};

export default DashboardPage;
