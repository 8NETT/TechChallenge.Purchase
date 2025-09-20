#!/usr/bin/env python3
import os
from pathlib import Path

nameToReplace = "TechChallenge.Users"
newName       = "TechChallenge.Purchases"
root          = Path(".").resolve()

# Pastas a ignorar (nome exato)
foldersToIgnore = {"bin", "obj", "debug", ".git", ".vs", "bkp", ".idea", ".vscode"}

# Arquivos a ignorar (nome exato)
filesToIgnore = {".gitignore", ".gitmodules", ".dockerignore", "rename.py"}

# Extensões tratadas como TEXTO (ajuste se precisar)
text_exts = {
    ".cs", ".csproj", ".sln", ".props", ".targets",
    ".json", ".yml", ".yaml", ".xml", ".md", ".txt",
    ".Dockerfile", ".editorconfig", ".gitignore"
}

# Extensões/binários a pular explicitamente (segurança extra)
binary_exts = {
    ".dll", ".exe", ".pdb", ".zip", ".tar", ".gz", ".7z",
    ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".ico", ".pdf",
    ".so", ".dylib", ".a", ".class", ".jar"
}

def is_text_file(path: Path) -> bool:
    # Prioriza lista branca/negra por extensão
    if path.suffix in text_exts:
        return True
    if path.suffix in binary_exts:
        return False
    # Heurística simples: tenta abrir como UTF-8 rápido
    try:
        with path.open("r", encoding="utf-8") as f:
            f.read(4096)
        return True
    except Exception:
        return False

def replace_in_file(path: Path):
    try:
        with path.open("r", encoding="utf-8") as fin:
            data = fin.read()
    except UnicodeDecodeError:
        # Se não for UTF-8 legível, pula
        return
    new_data = data.replace(nameToReplace, newName)
    if new_data != data:
        with path.open("w", encoding="utf-8") as fout:
            fout.write(new_data)

def main():
    files_to_rename   = []  # (old_path, new_path)
    dirs_to_rename    = []  # (old_path, new_path)

    # PASSO 1: varre e altera conteúdo dos arquivos de texto
    for current_dir, dirnames, filenames in os.walk(root, topdown=True):
        # filtra pastas ignoradas (in-place)
        dirnames[:] = [d for d in dirnames if d not in foldersToIgnore]

        cur = Path(current_dir)

        # processa arquivos
        for fname in filenames:
            if fname in filesToIgnore:
                continue
            fpath = cur / fname

            # pular links simbólicos
            if fpath.is_symlink():
                continue

            # pular binários óbvios
            if fpath.suffix in binary_exts:
                continue

            if is_text_file(fpath):
                replace_in_file(fpath)

            # agendar rename de arquivo se o nome contiver o alvo
            if nameToReplace in fname:
                new_name = fname.replace(nameToReplace, newName)
                files_to_rename.append((fpath, fpath.with_name(new_name)))

        # agendar rename de diretório se o nome contiver o alvo
        for d in dirnames:
            if nameToReplace in d:
                oldp = cur / d
                newp = cur / d.replace(nameToReplace, newName)
                dirs_to_rename.append((oldp, newp))

    # PASSO 2: renomeia arquivos (qualquer ordem, mas evitando colisão)
    for old, new in files_to_rename:
        if not new.exists():
            try:
                old.rename(new)
            except Exception as e:
                print(f"[WARN] Não foi possível renomear arquivo {old} -> {new}: {e}")

    # PASSO 3: renomeia diretórios do mais profundo para o mais raso
    # ordenar por profundidade decrescente
    dirs_to_rename.sort(key=lambda p: len(p[0].parts), reverse=True)
    for old, new in dirs_to_rename:
        if not new.exists():
            try:
                old.rename(new)
            except Exception as e:
                print(f"[WARN] Não foi possível renomear pasta {old} -> {new}: {e}")

if __name__ == "__main__":
    main()
