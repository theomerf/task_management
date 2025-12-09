export default function Home() {
    return (
        <div className="flex flex-col w-full gap-y-6">
            <div className="grid grid-cols-2 w-full gap-x-6">
                <div className="w-full flex flex-col gap-y-2 bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 rounded-lg p-4 shadow-lg">
                    <h2 className="text-xl text-center font-bold">Projelerim</h2>
                </div>
                <div className="w-full flex flex-col gap-y-2 bg-[rgb(var(--div))] border border-[rgb(var(--border))]/6 rounded-lg p-4 shadow-lg">
                    <h2 className="text-xl text-center font-bold">Görevlerim</h2>
                </div>
            </div>
        </div>
    );
}