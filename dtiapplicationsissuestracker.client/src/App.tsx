import { FormEvent, useEffect, useState } from 'react';
import './App.css';

interface Repository { id: number; name: string; }
interface Category { id: number; name: string; }
interface DataSource { id: number; name: string; }

interface Issue {
    id: number;
    repositoryId: number;
    categoryId: number;
    dataSourceId?: number;
    status: number;
    description: string;
    resolution?: string;
}

const statusOptions = [
    { value: 0, label: 'Open' },
    { value: 1, label: 'Resolved' }
];

function App() {
    const [repositories, setRepositories] = useState<Repository[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);
    const [dataSources, setDataSources] = useState<DataSource[]>([]);
    const [issues, setIssues] = useState<Issue[]>([]);

    const [repositoryId, setRepositoryId] = useState(0);
    const [newRepo, setNewRepo] = useState('');
    const [categoryId, setCategoryId] = useState(0);
    const [newCategory, setNewCategory] = useState('');
    const [dataSourceId, setDataSourceId] = useState<number | undefined>(undefined);
    const [newDataSource, setNewDataSource] = useState('');
    const [status, setStatus] = useState(0);
    const [description, setDescription] = useState('');
    const [resolution, setResolution] = useState('');

    useEffect(() => {
        loadAll();
    }, []);

    async function loadAll() {
        const [repos, cats, dss, iss] = await Promise.all([
            fetch('/api/issuetracker/repositories').then(r => r.json()),
            fetch('/api/issuetracker/categories').then(r => r.json()),
            fetch('/api/issuetracker/datasources').then(r => r.json()),
            fetch('/api/issuetracker/issues').then(r => r.json()),
        ]);
        setRepositories(repos);
        setCategories(cats);
        setDataSources(dss);
        setIssues(iss);
    }

    async function handleSubmit(e: FormEvent) {
        e.preventDefault();
        let repoId = repositoryId;
        if (!repoId && newRepo) {
            const r = await fetch('/api/issuetracker/repositories', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name: newRepo })
            });
            const repo = await r.json();
            repoId = repo.id;
            setRepositories([...repositories, repo]);
            setNewRepo('');
        }
        let catId = categoryId;
        if (!catId && newCategory) {
            const r = await fetch('/api/issuetracker/categories', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name: newCategory })
            });
            const cat = await r.json();
            catId = cat.id;
            setCategories([...categories, cat]);
            setNewCategory('');
        }
        let dsId = dataSourceId;
        if (categories.find(c => c.id === catId)?.name === 'Data') {
            if (!dsId && newDataSource) {
                const r = await fetch('/api/issuetracker/datasources', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ name: newDataSource })
                });
                const ds = await r.json();
                dsId = ds.id;
                setDataSources([...dataSources, ds]);
                setNewDataSource('');
            }
        } else {
            dsId = undefined;
        }

        const issueBody = {
            repositoryId: repoId,
            categoryId: catId,
            dataSourceId: dsId,
            status,
            description,
            resolution: resolution || undefined
        };

        const res = await fetch('/api/issuetracker/issues', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(issueBody)
        });
        if (res.ok) {
            const issue = await res.json();
            setIssues([...issues, issue]);
            setDescription('');
            setResolution('');
            setRepositoryId(0);
            setCategoryId(0);
            setDataSourceId(undefined);
        }
    }

    const selectedCategory = categories.find(c => c.id === categoryId)?.name;
    const showDataSource = selectedCategory === 'Data';

    return (
        <div className="container">
            <h1>Issue Tracker</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Repository: </label>
                    <select value={repositoryId} onChange={e => setRepositoryId(parseInt(e.target.value))}>
                        <option value={0}>--Add New--</option>
                        {repositories.map(r => <option key={r.id} value={r.id}>{r.name}</option>)}
                    </select>
                    {repositoryId === 0 && (
                        <input value={newRepo} onChange={e => setNewRepo(e.target.value)} placeholder="New repository" />
                    )}
                </div>
                <div>
                    <label>Category: </label>
                    <select value={categoryId} onChange={e => setCategoryId(parseInt(e.target.value))}>
                        <option value={0}>--Add New--</option>
                        {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                    </select>
                    {categoryId === 0 && (
                        <input value={newCategory} onChange={e => setNewCategory(e.target.value)} placeholder="New category" />
                    )}
                </div>
                {showDataSource && (
                    <div>
                        <label>Data Source: </label>
                        <select value={dataSourceId ?? 0} onChange={e => setDataSourceId(parseInt(e.target.value))}>
                            <option value={0}>--Add New--</option>
                            {dataSources.map(d => <option key={d.id} value={d.id}>{d.name}</option>)}
                        </select>
                        {(dataSourceId === undefined || dataSourceId === 0) && (
                            <input value={newDataSource} onChange={e => setNewDataSource(e.target.value)} placeholder="New data source" />
                        )}
                    </div>
                )}
                <div>
                    <label>Status: </label>
                    <select value={status} onChange={e => setStatus(parseInt(e.target.value))}>
                        {statusOptions.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
                    </select>
                </div>
                <div>
                    <label>Description: </label>
                    <textarea value={description} onChange={e => setDescription(e.target.value)} />
                </div>
                <div>
                    <label>Resolution: </label>
                    <textarea value={resolution} onChange={e => setResolution(e.target.value)} />
                </div>
                <button type="submit">Add Issue</button>
            </form>
            <h2>Existing Issues</h2>
            <ul>
                {issues.map(i => (
                    <li key={i.id}>
                        #{i.id} {repositories.find(r => r.id === i.repositoryId)?.name} - {categories.find(c => c.id === i.categoryId)?.name} - {statusOptions.find(s => s.value === i.status)?.label}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default App;
