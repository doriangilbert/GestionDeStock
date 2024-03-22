-- Insérer les marques
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Pilot');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Conté');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Foray');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Faber Castell');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Bic');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Velleda');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Pentel');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Estrée');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Elba');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Sans marque');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Scolex');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Viquel');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Calligraphe');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Clairefontaine');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Oxford');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Casio');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Maped');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Rubbermaid');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Office Dépôt');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Exacompta');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('CEP');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Pritt');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Niceday');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Han');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Emey');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Durable');
INSERT OR IGNORE INTO Marques (Nom) VALUES ('Parker');

-- Insérer les familles
INSERT OR IGNORE INTO Familles (Nom) VALUES ('Ecriture & Correction');
INSERT OR IGNORE INTO Familles (Nom) VALUES ('Papier');
INSERT OR IGNORE INTO Familles (Nom) VALUES ('Classement & Archivage');
INSERT OR IGNORE INTO Familles (Nom) VALUES ('Fournitures scolaires');
INSERT OR IGNORE INTO Familles (Nom) VALUES ('Accessoires de bureau');

-- Insérer les sous-familles
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Ecriture & Correction'), 'Stylos, feutres & rollers');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Ecriture & Correction'), 'Crayons & Porte-mines');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Ecriture & Correction'), 'Marqueurs');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Papier'), 'Blocs & Coffrets de correspondance');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Papier'), 'Papier copieur & multifonction blanc');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Papier'), 'Papier laser');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Classement & Archivage'), 'Boites de classement en plastique');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Classement & Archivage'), 'Boîtes transfert');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Accessoires de bureau'), 'Corbeilles à papier');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Accessoires de bureau'), 'Pots à crayons');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Accessoires de bureau'), 'Sous-mains');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Fournitures scolaires'), 'Informatique et calculatrices');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Fournitures scolaires'), 'Création et géométrie');
INSERT OR IGNORE INTO SousFamilles (RefFamille, Nom) VALUES ((SELECT RefFamille FROM Familles WHERE Nom = 'Fournitures scolaires'), 'Agendas et calendriers');

-- Insérer les articles
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0010087', '12 Billes gel G1 Pilot Bleu', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Stylos, feutres & rollers'), (SELECT RefMarque FROM Marques WHERE Nom = 'Pilot'), 7.05, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0000019', '12 Crayons à papier Tradition HB', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Crayons & Porte-mines'), (SELECT RefMarque FROM Marques WHERE Nom = 'Conté'), 5.09, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F1646150', '12 Crayons graphite Foray HB', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Crayons & Porte-mines'), (SELECT RefMarque FROM Marques WHERE Nom = 'Foray'), 2.69, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0346438', '3 crayons à papier - Faber Castell - Grip 2001 HB', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Crayons & Porte-mines'), (SELECT RefMarque FROM Marques WHERE Nom = 'Faber Castell'), 2.69, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0013378', '4 crayons à papier - Bic - Evolution HB', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Crayons & Porte-mines'), (SELECT RefMarque FROM Marques WHERE Nom = 'Bic'), 0.89, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0333613', '6 x Marqueurs - Velleda - 1781bis', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Marqueurs'), (SELECT RefMarque FROM Marques WHERE Nom = 'Velleda'), 7.39, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0354305', '7 marqueurs à vitres assortis Pentel Wet Erase', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Marqueurs'), (SELECT RefMarque FROM Marques WHERE Nom = 'Pentel'), 19.49, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0005477', 'Bloc Estrée A4 75g Uni', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Blocs & Coffrets de correspondance'), (SELECT RefMarque FROM Marques WHERE Nom = 'Estrée'), 1.99, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0351775', 'Boîte de classement - 24 x 32 cm - personnalisable bleu translucide dos 40 mm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boites de classement en plastique'), (SELECT RefMarque FROM Marques WHERE Nom = 'Elba'), 4.75, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0351779', 'Boîte de classement - 24 x 32 cm - personnalisable vert translucide dos 40 mm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boites de classement en plastique'), (SELECT RefMarque FROM Marques WHERE Nom = 'Elba'), 4.75, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5613471', 'Boîte de classement - polypropylène - dos 25 mm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boites de classement en plastique'), (SELECT RefMarque FROM Marques WHERE Nom = 'Sans marque'), 2.79, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0012290', 'Boîte transfert - Scolex - havane - dos 90mm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boîtes transfert'), (SELECT RefMarque FROM Marques WHERE Nom = 'Scolex'), 3.59, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0331333', 'Boîte transfert Scolex dos 120mm bleu marine', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boîtes transfert'), (SELECT RefMarque FROM Marques WHERE Nom = 'Scolex'), 6.14, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0012295', 'Boîte transfert Scolex dos 90mm bleu marine', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boîtes transfert'), (SELECT RefMarque FROM Marques WHERE Nom = 'Scolex'), 3.59, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0012294', 'Boîte transfert Scolex dos 90mm vert', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boîtes transfert'), (SELECT RefMarque FROM Marques WHERE Nom = 'Scolex'), 3.59, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0319838', 'Boîte de classement Viquel polypropylène 8/10e Dos 80 mm Rouge', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Boites de classement en plastique'), (SELECT RefMarque FROM Marques WHERE Nom = 'Viquel'), 4.75, 0);

INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0328476', 'Cahier de texte Clairefontaine 144 pages à spirales', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Agendas et calendriers'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 2.99, 0);


INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F6116922', 'Calculatrice scientifique - CASIO - FX Junior Plus - 19,5 x 78 x 155 mm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Informatique et calculatrices'), (SELECT RefMarque FROM Marques WHERE Nom = 'Casio'), 9.02, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0006804', 'Clairalfa - carton de 5 ramettes de 500 feuilles A4 80g', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier copieur & multifonction blanc'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 21.45, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0000269', 'Clairalfa - ramette 500 feuilles A4 80g', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier copieur & multifonction blanc'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 4.69, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0300012', 'Clairefontaine - 250 feuilles Digital Color Printing blanches A4 160g', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier laser'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 7.59, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0020064', 'Clairefontaine - 500 feuilles Digital Color Printing blanches A4 100g', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier laser'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 7.69, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0021375', 'Clairefontaine - 500 feuilles Digital Color Printing ivoire A4 100g', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier laser'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 11.49, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0382778', 'Clairefontaine - Carton de 5 ramettes de 500 feuilles Digital Color Printing blanches A4 100g', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier laser'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 34.95, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5448249', 'Coffret de correspondance - Clairefontaine - Pollen - 40 cartes + 40 enveloppes - bleu', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Blocs & Coffrets de correspondance'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 9.99, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5448222', 'Coffret de correspondance - Clairefontaine - Pollen - 40 cartes + 40 enveloppes - hiver', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Blocs & Coffrets de correspondance'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 13.49, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5448339', 'Coffret de correspondance - Clairefontaine - Pollen - 40 cartes + 40 enveloppes - vert', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Blocs & Coffrets de correspondance'), (SELECT RefMarque FROM Marques WHERE Nom = 'Clairefontaine'), 9.99, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0017641', 'Corbeille à papier - métal - 18 litres - noir', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Corbeilles à papier'), (SELECT RefMarque FROM Marques WHERE Nom = 'Sans marque'), 23.89, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0015773', 'Corbeille à papier Ecoline CEP bleue', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Corbeilles à papier'), (SELECT RefMarque FROM Marques WHERE Nom = 'CEP'), 2.29, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0016578', 'Corbeille à papier rectangulaire HAN noire', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Corbeilles à papier'), (SELECT RefMarque FROM Marques WHERE Nom = 'Han'), 6.19, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0368396', 'Equerre 60° - Maped - Flex - 21cm incassable', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Création et géométrie'), (SELECT RefMarque FROM Marques WHERE Nom = 'Maped'), 1.09, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0358457', 'Multipot à crayons rotatif - Rubbermaid - noir', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Pots à crayons'), (SELECT RefMarque FROM Marques WHERE Nom = 'Rubbermaid'), 24.89, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0391110', 'Office DEPOT - carton de 5 ramettes de 500 feuilles Office DEPOT Everyday A4 80g/m²', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier copieur & multifonction blanc'), (SELECT RefMarque FROM Marques WHERE Nom = 'Office Dépôt'), 17.45, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F1453759', 'Office DEPOT - Ramette 500 feuilles Office DEPOT Everyday A4 80g/m²', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Papier copieur & multifonction blanc'), (SELECT RefMarque FROM Marques WHERE Nom = 'Office Dépôt'), 3.79, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0320260', 'Organiseur en cuir pleine fleur - Exacompta - Cali Exatime 21 - noir - 23 x 19 cm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Agendas et calendriers'), (SELECT RefMarque FROM Marques WHERE Nom = 'Exacompta'), 100.33, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5422829', 'Pot à crayons - Cep - Acrylight - transparent', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Pots à crayons'), (SELECT RefMarque FROM Marques WHERE Nom = 'CEP'), 8.99, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0358458', 'Pot à crayons multipot fixe bleu', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Pots à crayons'), (SELECT RefMarque FROM Marques WHERE Nom = 'Sans marque'), 2.69, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0308391', 'Pot à crayons recyclé - Cep - Green Spirit', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Pots à crayons'), (SELECT RefMarque FROM Marques WHERE Nom = 'CEP'), 5.01, 0);

INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5743512', 'Règle transparente - Niceday - 1 biseau - 40 cm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Création et géométrie'), (SELECT RefMarque FROM Marques WHERE Nom = 'Niceday'), 0.66, 0);

INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0388467', 'Sous-main à rabat 40 x 60 cm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Sous-mains'), (SELECT RefMarque FROM Marques WHERE Nom = 'Emey'), 46.79, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0365764', 'Sous-main France 36x60cm', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Sous-mains'), (SELECT RefMarque FROM Marques WHERE Nom = 'Viquel'), 6.49, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F3754287', 'Sous-main géographique - Durable - Monde', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Sous-mains'), (SELECT RefMarque FROM Marques WHERE Nom = 'Durable'), 6.49, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5223348', 'Stylo à bille - Parker - Urban Premium - corps acier - Ebène', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Stylos, feutres & rollers'), (SELECT RefMarque FROM Marques WHERE Nom = 'Parker'), 25.49, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F5223222', 'Stylo à bille - Parker - Urban Premium - corps acier - Perle', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Stylos, feutres & rollers'), (SELECT RefMarque FROM Marques WHERE Nom = 'Parker'), 25.49, 0);
INSERT INTO Articles (RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES ('F0333037', 'Stylo-bille à encre gel - Pilot - B2P - noir', (SELECT RefSousFamille FROM SousFamilles WHERE Nom = 'Stylos, feutres & rollers'), (SELECT RefMarque FROM Marques WHERE Nom = 'Pilot'), 2.59, 0);
